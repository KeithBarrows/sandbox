using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WeatherStation.Data;
using WeatherStation.Data.Entities;
using WeatherStation.Data.Options;

namespace WeatherStation.Web.Services;

// Startup task, runs once per app start, once per configured location:
//  - If that location's table rows are empty, backfill roughly a year of hourly history
//    from Open-Meteo's free archive API so the 1-month/1-year charts show real pressure
//    history immediately instead of starting blank.
//  - Otherwise, self-heal: scan recent history for ANY gap larger than expected -- not
//    just staleness since the newest row, but a hole anywhere in the last ~90 days
//    (e.g. the archive backfill's ~5-day lag left a hole before live collection ever
//    started, or the service was down for a stretch in the middle of otherwise-current
//    data). Whichever is earliest, fill forward from there using the forecast API's
//    `past_days` parameter, which -- unlike the archive/reanalysis API -- has no
//    multi-day lag, so it can cover right up to "now".
public sealed class HistoricalBackfillService(
    IServiceScopeFactory scopeFactory,
    IOptions<List<LocationOptions>> locationOptions,
    IOptions<CollectorOptions> collectorOptions,
    WeatherUpdateNotifier notifier,
    ILogger<HistoricalBackfillService> logger) : BackgroundService
{
    private const decimal HPaToInHg = 0.0295299830714m;
    private const int ArchiveDataLagDays = 5;
    private const int MaxAttempts = 5;
    private static readonly TimeSpan GapFillThreshold = TimeSpan.FromHours(2);
    private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(15);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var location in locationOptions.Value)
        {
            await BackfillLocationWithRetryAsync(location, stoppingToken);
        }
    }

    // Runs concurrently with WeatherCollectorService's first poll (for the same
    // location), so a duplicate-key violation on (LocationKey, ObservedAtUtc) is an
    // expected race (both trying to insert the same just-arrived hour), not a fatal
    // condition -- retrying re-reads what's already in the table and simply won't
    // re-insert it the second time. Anything still failing after 5 attempts is a real
    // problem, so it's logged and left for the next service start rather than crashing
    // the host (the default BackgroundService behavior).
    private async Task BackfillLocationWithRetryAsync(LocationOptions location, CancellationToken stoppingToken)
    {
        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();

                var hasAnyData = await db.WeatherObservations.AnyAsync(o => o.LocationKey == location.Key, stoppingToken);

                if (!hasAnyData)
                {
                    await BackfillFullHistoryAsync(db, scope.ServiceProvider, location, stoppingToken);
                }
                else
                {
                    await FillGapsAsync(db, scope.ServiceProvider, location, stoppingToken);
                }

                return;
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Historical backfill for {LocationKey} canceled; service is stopping.", location.Key);
                return;
            }
            catch (Exception ex) when (attempt < MaxAttempts)
            {
                logger.LogWarning(ex,
                    "Historical backfill attempt {Attempt}/{MaxAttempts} for {LocationKey} failed; retrying in {Delay}...",
                    attempt, MaxAttempts, location.Key, RetryDelay);
                await Task.Delay(RetryDelay, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Historical backfill for {LocationKey} failed after {MaxAttempts} attempts; giving up for this run.",
                    location.Key, MaxAttempts);
            }
        }
    }

    private async Task BackfillFullHistoryAsync(WeatherDbContext db, IServiceProvider services, LocationOptions location, CancellationToken ct)
    {
        var backfillDays = Math.Max(1, collectorOptions.Value.BackfillDays);
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-ArchiveDataLagDays);
        var startDate = endDate.AddDays(-backfillDays);

        logger.LogInformation("Backfilling historical weather for {LocationKey} from {Start} to {End}...", location.Key, startDate, endDate);

        var archiveClient = services.GetRequiredService<OpenMeteoArchiveClient>();
        var hourly = await archiveClient.GetHourlyHistoryAsync(
            location.Latitude, location.Longitude, startDate, endDate, ct);

        if (hourly is null || hourly.Time.Count == 0)
        {
            logger.LogWarning("Historical backfill for {LocationKey} returned no data; skipping.", location.Key);
            return;
        }

        var now = DateTime.UtcNow;
        var rows = new List<WeatherObservation>(hourly.Time.Count);
        for (var i = 0; i < hourly.Time.Count; i++)
        {
            rows.Add(MapRow(hourly, i, location.Key, "backfill", now));
        }

        db.WeatherObservations.AddRange(rows);
        await db.SaveChangesAsync(ct);
        notifier.NotifyUpdated();

        logger.LogInformation("Historical backfill for {LocationKey} complete: inserted {Count} rows.", location.Key, rows.Count);
    }

    private async Task FillGapsAsync(WeatherDbContext db, IServiceProvider services, LocationOptions location, CancellationToken ct)
    {
        var nowUtc = DateTime.UtcNow;
        var windowStart = nowUtc.AddDays(-90);

        var timestamps = await db.WeatherObservations
            .Where(o => o.LocationKey == location.Key && o.ObservedAtUtc >= windowStart)
            .OrderBy(o => o.ObservedAtUtc)
            .Select(o => o.ObservedAtUtc)
            .ToListAsync(ct);

        // Append "now" as a sentinel so a trailing gap (nothing recorded recently,
        // e.g. the service was down) is detected by the same scan as an internal hole.
        timestamps.Add(nowUtc);

        DateTime? gapStartUtc = null;
        for (var i = 0; i < timestamps.Count - 1; i++)
        {
            if (timestamps[i + 1] - timestamps[i] > GapFillThreshold)
            {
                gapStartUtc = timestamps[i];
                break; // earliest gap first
            }
        }

        if (gapStartUtc is null)
        {
            logger.LogInformation("No gap found in the last 90 days of history for {LocationKey}; nothing to fill.", location.Key);
            return;
        }

        var pastDays = Math.Clamp((int)Math.Ceiling((nowUtc - gapStartUtc.Value).TotalDays) + 1, 1, 92);
        logger.LogInformation(
            "Found a gap for {LocationKey} starting after {GapStart}; filling via Open-Meteo's recent-hourly (past_days={PastDays})...",
            location.Key, gapStartUtc, pastDays);

        var client = services.GetRequiredService<OpenMeteoClient>();
        var hourly = await client.GetRecentHourlyAsync(location.Latitude, location.Longitude, pastDays, ct);

        if (hourly is null || hourly.Time.Count == 0)
        {
            logger.LogWarning("Gap-fill request for {LocationKey} returned no data; the gap will remain until the next successful start.", location.Key);
            return;
        }

        var existingSet = (await db.WeatherObservations
            .Where(o => o.LocationKey == location.Key && o.ObservedAtUtc > gapStartUtc.Value)
            .Select(o => o.ObservedAtUtc)
            .ToListAsync(ct)).ToHashSet();

        var now = DateTime.UtcNow;
        var rows = new List<WeatherObservation>();
        for (var i = 0; i < hourly.Time.Count; i++)
        {
            var observedAtUtc = DateTime.SpecifyKind(hourly.Time[i], DateTimeKind.Utc);

            // The forecast API's response spans past_days *and* forecast_days=1, so it
            // includes hours that haven't happened yet -- never store those as if they
            // were real observations.
            if (observedAtUtc <= gapStartUtc.Value || observedAtUtc > now || existingSet.Contains(observedAtUtc)) continue;

            rows.Add(MapRow(hourly, i, location.Key, "gapfill", now));
        }

        if (rows.Count == 0)
        {
            logger.LogInformation("Gap-fill for {LocationKey} found no new rows to insert.", location.Key);
            return;
        }

        db.WeatherObservations.AddRange(rows);
        await db.SaveChangesAsync(ct);
        notifier.NotifyUpdated();

        logger.LogInformation("Gap-fill for {LocationKey} complete: inserted {Count} rows.", location.Key, rows.Count);
    }

    private static WeatherObservation MapRow(OpenMeteoArchiveHourly hourly, int i, string locationKey, string source, DateTime createdAtUtc) => new()
    {
        LocationKey = locationKey,
        ObservedAtUtc = DateTime.SpecifyKind(hourly.Time[i], DateTimeKind.Utc),
        TempF = hourly.TemperatureF[i],
        FeelsLikeF = hourly.ApparentTemperatureF[i],
        DewpointF = hourly.DewPointF[i],
        HumidityPct = hourly.HumidityPct[i],
        WindSpeedMph = hourly.WindSpeedMph[i],
        WindGustMph = hourly.WindGustsMph[i],
        UvIndex = i < hourly.UvIndex.Count ? hourly.UvIndex[i] : null,
        PressureInHg = hourly.PressureMslHPa[i] is { } hpa ? (decimal)hpa * HPaToInHg : null,
        PrecipInPerHr = hourly.PrecipitationIn[i] is { } precip ? (decimal)precip : null,
        Source = source,
        CreatedAtUtc = createdAtUtc,
    };
}
