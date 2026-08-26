namespace WeatherStation.Data.Querying;

public enum BucketSize { Raw, Hour, Day }

public sealed record BucketedPoint(
    DateTime BucketStartUtc,
    decimal? PressureMinInHg,
    decimal? PressureMaxInHg,
    decimal? PressureAvgInHg,
    double? TempAvgF,
    double? FeelsLikeAvgF,
    double? DewpointAvgF,
    double? HumidityAvgPct,
    double? WindSpeedAvgMph,
    double? WindGustMaxMph,
    decimal? PrecipAvgInPerHr,
    double? UvAvg);

// Pressure-only counterpart for secondary locations, which the dashboard overlays onto
// the primary location's pressure chart but otherwise never displays.
public sealed record PressureBucketPoint(
    DateTime BucketStartUtc,
    decimal? PressureMinInHg,
    decimal? PressureMaxInHg,
    decimal? PressureAvgInHg);
