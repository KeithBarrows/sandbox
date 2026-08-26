using Microsoft.EntityFrameworkCore;
using WeatherStation.Data.Entities;

namespace WeatherStation.Data.Querying;

public sealed class WeatherQueryService(WeatherDbContext db) : IWeatherQueryService
{
    public Task<WeatherObservation?> GetLatestAsync(string locationKey, CancellationToken ct) =>
        db.WeatherObservations
            .Where(o => o.LocationKey == locationKey)
            .OrderByDescending(o => o.ObservedAtUtc)
            .FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<BucketedPoint>> GetBucketedAsync(
        string locationKey, DateTime fromUtc, DateTime toUtc, BucketSize bucketSize, CancellationToken ct)
    {
        if (bucketSize == BucketSize.Raw)
        {
            return await db.WeatherObservations
                .Where(o => o.LocationKey == locationKey && o.ObservedAtUtc >= fromUtc && o.ObservedAtUtc <= toUtc)
                .OrderBy(o => o.ObservedAtUtc)
                .Select(o => new BucketedPoint(
                    o.ObservedAtUtc,
                    o.PressureInHg, o.PressureInHg, o.PressureInHg,
                    o.TempF, o.FeelsLikeF, o.DewpointF,
                    o.HumidityPct, o.WindSpeedMph, o.WindGustMph,
                    o.PrecipInPerHr, o.UvIndex))
                .ToListAsync(ct);
        }

        var truncUnit = bucketSize == BucketSize.Hour ? "hour" : "day";

        // Min/Max/Avg for pressure specifically so a bucket's swing survives aggregation
        // as a band rather than being smoothed into a single average line.
        return await db.Database.SqlQuery<BucketedPoint>($"""
            SELECT
                date_trunc({truncUnit}, "ObservedAtUtc") AS "BucketStartUtc",
                MIN("PressureInHg")   AS "PressureMinInHg",
                MAX("PressureInHg")   AS "PressureMaxInHg",
                AVG("PressureInHg")   AS "PressureAvgInHg",
                AVG("TempF")          AS "TempAvgF",
                AVG("FeelsLikeF")     AS "FeelsLikeAvgF",
                AVG("DewpointF")      AS "DewpointAvgF",
                AVG("HumidityPct"::float) AS "HumidityAvgPct",
                AVG("WindSpeedMph")   AS "WindSpeedAvgMph",
                MAX("WindGustMph")    AS "WindGustMaxMph",
                AVG("PrecipInPerHr")  AS "PrecipAvgInPerHr",
                AVG("UvIndex")        AS "UvAvg"
            FROM "WeatherObservations"
            WHERE "LocationKey" = {locationKey} AND "ObservedAtUtc" >= {fromUtc} AND "ObservedAtUtc" <= {toUtc}
            GROUP BY 1
            ORDER BY 1
            """).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<PressureBucketPoint>> GetPressureBucketedAsync(
        string locationKey, DateTime fromUtc, DateTime toUtc, BucketSize bucketSize, CancellationToken ct)
    {
        if (bucketSize == BucketSize.Raw)
        {
            return await db.WeatherObservations
                .Where(o => o.LocationKey == locationKey && o.ObservedAtUtc >= fromUtc && o.ObservedAtUtc <= toUtc)
                .OrderBy(o => o.ObservedAtUtc)
                .Select(o => new PressureBucketPoint(o.ObservedAtUtc, o.PressureInHg, o.PressureInHg, o.PressureInHg))
                .ToListAsync(ct);
        }

        var truncUnit = bucketSize == BucketSize.Hour ? "hour" : "day";

        return await db.Database.SqlQuery<PressureBucketPoint>($"""
            SELECT
                date_trunc({truncUnit}, "ObservedAtUtc") AS "BucketStartUtc",
                MIN("PressureInHg") AS "PressureMinInHg",
                MAX("PressureInHg") AS "PressureMaxInHg",
                AVG("PressureInHg") AS "PressureAvgInHg"
            FROM "WeatherObservations"
            WHERE "LocationKey" = {locationKey} AND "ObservedAtUtc" >= {fromUtc} AND "ObservedAtUtc" <= {toUtc}
            GROUP BY 1
            ORDER BY 1
            """).ToListAsync(ct);
    }
}
