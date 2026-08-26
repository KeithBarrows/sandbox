namespace WeatherStation.Web.Components.Dashboard;

// BucketedPoint converted to the household's local time, for chart display.
public sealed record ChartRow(
    DateTime LocalTime,
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
