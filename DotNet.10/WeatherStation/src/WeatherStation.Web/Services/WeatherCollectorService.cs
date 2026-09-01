using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WeatherStation.Data;
using WeatherStation.Data.Entities;
using WeatherStation.Data.Options;

namespace WeatherStation.Web.Services;

public sealed class WeatherCollectorService(
    IServiceScopeFactory scopeFactory,
    IOptions<List<LocationOptions>> locationOptions,
    IOptions<CollectorOptions> collectorOptions,
    WeatherUpdateNotifier notifier,
    ILogger<WeatherCollectorService> logger) : BackgroundService
{
    private const decimal HPaToInHg = 0.0295299830714m;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var interval = TimeSpan.FromMinutes(Math.Max(1, collectorOptions.Value.PollIntervalMinutes));
        using var timer = new PeriodicTimer(interval);

        do
        {
            // Sequential, not parallel: these share the same rate-limited free Open-Meteo
            // API, and a handful of locations polled a second or two apart is irrelevant
            // next to the 15-minute poll interval.
            foreach (var location in locationOptions.Value)
            {
                try
                {
                    await PollAsync(location, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Weather collector poll failed for {LocationKey}.", location.Key);
                }
            }
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async Task PollAsync(LocationOptions location, CancellationToken ct)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var client = scope.ServiceProvider.GetRequiredService<OpenMeteoClient>();
        var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();

        var (current, forecast) = await client.GetCurrentAsync(location.Latitude, location.Longitude, ct);
        if (current is null) return;

        var observedAtUtc = DateTime.SpecifyKind(current.Time, DateTimeKind.Utc);

        var exists = await db.WeatherObservations.AnyAsync(o => o.LocationKey == location.Key && o.ObservedAtUtc == observedAtUtc, ct);
        if (exists) return; // Open-Meteo hasn't produced a newer reading since our last poll.

        var now = DateTime.UtcNow;

        db.WeatherObservations.Add(new WeatherObservation
        {
            LocationKey = location.Key,
            ObservedAtUtc = observedAtUtc,
            TempF = current.TemperatureF,
            FeelsLikeF = current.ApparentTemperatureF,
            DewpointF = current.DewPointF,
            HumidityPct = current.HumidityPct,
            WindSpeedMph = current.WindSpeedMph,
            WindGustMph = current.WindGustsMph,
            WindDirDeg = current.WindDirectionDeg,
            UvIndex = current.UvIndex,
            PressureInHg = current.PressureMslHPa is { } hpa ? (decimal)hpa * HPaToInHg : null,
            PrecipInPerHr = current.PrecipitationIn is { } precip ? (decimal)precip : null,
            Source = "live",
            CreatedAtUtc = now,
        });

        // Silently tracked for a future predictive model -- no UI surface for this yet.
        // Tied to observedAtUtc via IssuedAtUtc rather than checked separately, since a
        // new WeatherObservation row already guarantees this poll cycle is new.
        if (forecast is not null)
        {
            for (var i = 0; i < forecast.Time.Count; i++)
            {
                db.ForecastObservations.Add(new ForecastObservation
                {
                    LocationKey = location.Key,
                    IssuedAtUtc = observedAtUtc,
                    ForecastForUtc = DateTime.SpecifyKind(forecast.Time[i], DateTimeKind.Utc),
                    TempF = forecast.TemperatureF[i],
                    FeelsLikeF = forecast.ApparentTemperatureF[i],
                    DewpointF = forecast.DewPointF[i],
                    HumidityPct = forecast.HumidityPct[i],
                    WindSpeedMph = forecast.WindSpeedMph[i],
                    WindGustMph = forecast.WindGustsMph[i],
                    UvIndex = i < forecast.UvIndex.Count ? forecast.UvIndex[i] : null,
                    PressureInHg = forecast.PressureMslHPa[i] is { } fHpa ? (decimal)fHpa * HPaToInHg : null,
                    PrecipInPerHr = forecast.PrecipitationIn[i] is { } fPrecip ? (decimal)fPrecip : null,
                    CreatedAtUtc = now,
                });
            }
        }

        await db.SaveChangesAsync(ct);

        notifier.NotifyUpdated();
    }
}
