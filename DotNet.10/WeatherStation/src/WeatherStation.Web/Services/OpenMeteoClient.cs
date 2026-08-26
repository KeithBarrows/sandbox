using System.Text.Json.Serialization;

namespace WeatherStation.Web.Services;

public sealed class OpenMeteoClient(HttpClient httpClient, ILogger<OpenMeteoClient> logger)
{
    private const string CurrentFields =
        "temperature_2m,relative_humidity_2m,apparent_temperature,dew_point_2m," +
        "pressure_msl,wind_speed_10m,wind_gusts_10m,wind_direction_10m,precipitation,uv_index";

    private const string HourlyFields =
        "temperature_2m,relative_humidity_2m,apparent_temperature,dew_point_2m," +
        "pressure_msl,wind_speed_10m,wind_gusts_10m,precipitation,uv_index";

    // Fetches current conditions plus the next 6 hours of hourly forecast in a single
    // request (Open-Meteo allows combining `current=`, `hourly=`, and `forecast_hours=`
    // in one call) -- the forecast rows are stored silently alongside the actual
    // reading so a future predictive model can compare "what was forecast N hours out"
    // against what actually happened.
    public async Task<(OpenMeteoCurrent? Current, OpenMeteoArchiveHourly? Forecast)> GetCurrentAsync(double lat, double lon, CancellationToken ct)
    {
        var url = $"v1/forecast?latitude={lat}&longitude={lon}&current={CurrentFields}" +
                  $"&hourly={HourlyFields}&forecast_hours=6" +
                  "&temperature_unit=fahrenheit&wind_speed_unit=mph&precipitation_unit=inch";

        // One retry for a transient connection blip (e.g. a stale pooled connection) so
        // a single hiccup doesn't cost a full 15-minute wait until the next poll tick.
        for (var attempt = 1; attempt <= 2; attempt++)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<OpenMeteoForecastResponse>(url, ct);
                if (response?.Current is null)
                    logger.LogWarning("Open-Meteo returned no current-conditions block.");
                return (response?.Current, response?.Hourly);
            }
            catch (HttpRequestException ex)
            {
                if (attempt == 2)
                {
                    logger.LogWarning(ex, "Open-Meteo current-conditions request failed (after retry).");
                    return (null, null);
                }
                logger.LogInformation(ex, "Open-Meteo current-conditions request failed; retrying once.");
                await Task.Delay(TimeSpan.FromSeconds(5), ct);
            }
        }

        return (null, null);
    }

    // Unlike the archive/reanalysis API (~5 day lag), the forecast API's `past_days`
    // returns real recent hourly data with no lag -- used to self-heal any gap left by
    // downtime (the app not running, a reboot before the service auto-starts, etc.).
    public async Task<OpenMeteoArchiveHourly?> GetRecentHourlyAsync(double lat, double lon, int pastDays, CancellationToken ct)
    {
        pastDays = Math.Clamp(pastDays, 1, 92);
        var url = $"v1/forecast?latitude={lat}&longitude={lon}&past_days={pastDays}&forecast_days=1" +
                  $"&hourly={HourlyFields}&temperature_unit=fahrenheit&wind_speed_unit=mph&precipitation_unit=inch";
        try
        {
            var response = await httpClient.GetFromJsonAsync<OpenMeteoArchiveResponse>(url, ct);
            return response?.Hourly;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Open-Meteo recent-hourly gap-fill request failed.");
            return null;
        }
    }
}

public sealed class OpenMeteoForecastResponse
{
    [JsonPropertyName("current")]
    public OpenMeteoCurrent? Current { get; set; }

    [JsonPropertyName("hourly")]
    public OpenMeteoArchiveHourly? Hourly { get; set; }
}

public sealed class OpenMeteoCurrent
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("temperature_2m")]
    public double? TemperatureF { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public int? HumidityPct { get; set; }

    [JsonPropertyName("apparent_temperature")]
    public double? ApparentTemperatureF { get; set; }

    [JsonPropertyName("dew_point_2m")]
    public double? DewPointF { get; set; }

    [JsonPropertyName("pressure_msl")]
    public double? PressureMslHPa { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public double? WindSpeedMph { get; set; }

    [JsonPropertyName("wind_gusts_10m")]
    public double? WindGustsMph { get; set; }

    [JsonPropertyName("wind_direction_10m")]
    public int? WindDirectionDeg { get; set; }

    [JsonPropertyName("precipitation")]
    public double? PrecipitationIn { get; set; }

    [JsonPropertyName("uv_index")]
    public double? UvIndex { get; set; }
}
