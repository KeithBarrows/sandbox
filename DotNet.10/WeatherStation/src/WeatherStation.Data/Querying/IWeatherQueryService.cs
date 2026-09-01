using WeatherStation.Data.Entities;

namespace WeatherStation.Data.Querying;

public interface IWeatherQueryService
{
    Task<WeatherObservation?> GetLatestAsync(string locationKey, CancellationToken ct);

    Task<IReadOnlyList<BucketedPoint>> GetBucketedAsync(
        string locationKey, DateTime fromUtc, DateTime toUtc, BucketSize bucketSize, CancellationToken ct);

    Task<IReadOnlyList<PressureBucketPoint>> GetPressureBucketedAsync(
        string locationKey, DateTime fromUtc, DateTime toUtc, BucketSize bucketSize, CancellationToken ct);
}
