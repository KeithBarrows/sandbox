namespace WeatherStation.Data.Entities;

// One row per (issued-at, forecast-hour) pair -- e.g. the poll at 3:00pm might record
// forecasts for 4pm, 5pm, ... 9pm. Kept alongside the actual WeatherObservation rows so
// a future predictive model can compare "what was forecast N hours out" against "what
// actually happened" at that same ForecastForUtc timestamp.
public class ForecastObservation
{
    public long Id { get; set; }

    public string LocationKey { get; set; } = "";

    // Matches the WeatherObservation.ObservedAtUtc from the same poll cycle that
    // produced this forecast batch -- ties a set of forecast rows to the actual
    // reading taken at the same time.
    public DateTime IssuedAtUtc { get; set; }

    public DateTime ForecastForUtc { get; set; }

    public double? TempF { get; set; }
    public double? FeelsLikeF { get; set; }
    public double? DewpointF { get; set; }
    public int? HumidityPct { get; set; }
    public double? WindSpeedMph { get; set; }
    public double? WindGustMph { get; set; }
    public double? UvIndex { get; set; }

    public decimal? PressureInHg { get; set; }
    public decimal? PrecipInPerHr { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
