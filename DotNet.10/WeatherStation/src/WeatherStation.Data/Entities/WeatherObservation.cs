namespace WeatherStation.Data.Entities;

public class WeatherObservation
{
    public long Id { get; set; }

    public string LocationKey { get; set; } = "";

    public DateTime ObservedAtUtc { get; set; }

    public double? TempF { get; set; }
    public double? FeelsLikeF { get; set; }
    public double? DewpointF { get; set; }
    public int? HumidityPct { get; set; }
    public double? WindSpeedMph { get; set; }
    public double? WindGustMph { get; set; }
    public int? WindDirDeg { get; set; }
    public double? UvIndex { get; set; }

    // Pressure and precip need exact arithmetic (min/max aggregation for pressure swings,
    // running precip totals) so they use decimal rather than double.
    public decimal? PressureInHg { get; set; }
    public decimal? PrecipInPerHr { get; set; }

    public string Source { get; set; } = "live";

    public DateTime CreatedAtUtc { get; set; }
}
