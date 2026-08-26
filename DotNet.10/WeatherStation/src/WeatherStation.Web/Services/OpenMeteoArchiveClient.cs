using System.Text.Json.Serialization;

namespace WeatherStation.Web.Services;

public sealed class OpenMeteoArchiveClient(HttpClient httpClient, ILogger<OpenMeteoArchiveClient> logger)
{
    private const string HourlyFields =
        "temperature_2m,relative_humidity_2m,apparent_temperature,dew_point_2m," +
        "pressure_msl,wind_speed_10m,wind_gusts_10m,precipitation";

    public async Task<OpenMeteoArchiveHourly?> GetHourlyHistoryAsync(
        double lat, double lon, DateOnly startDate, DateOnly endDate, CancellationToken ct)
    {
        var url = $"v1/archive?latitude={lat}&longitude={lon}" +
                  $"&start_date={startDate:yyyy-MM-dd}&end_date={endDate:yyyy-MM-dd}" +
                  $"&hourly={HourlyFields}" +
                  "&temperature_unit=fahrenheit&wind_speed_unit=mph&precipitation_unit=inch";
        try
        {
            var response = await httpClient.GetFromJsonAsync<OpenMeteoArchiveResponse>(url, ct);
            return response?.Hourly;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Open-Meteo archive request failed.");
            return null;
        }
    }
}

public sealed class OpenMeteoArchiveResponse
{
    [JsonPropertyName("hourly")]
    public OpenMeteoArchiveHourly? Hourly { get; set; }
}

public sealed class OpenMeteoArchiveHourly
{
    [JsonPropertyName("time")]
    public List<DateTime> Time { get; set; } = [];

    [JsonPropertyName("temperature_2m")]
    public List<double?> TemperatureF { get; set; } = [];

    [JsonPropertyName("relative_humidity_2m")]
    public List<int?> HumidityPct { get; set; } = [];

    [JsonPropertyName("apparent_temperature")]
    public List<double?> ApparentTemperatureF { get; set; } = [];

    [JsonPropertyName("dew_point_2m")]
    public List<double?> DewPointF { get; set; } = [];

    [JsonPropertyName("pressure_msl")]
    public List<double?> PressureMslHPa { get; set; } = [];

    [JsonPropertyName("wind_speed_10m")]
    public List<double?> WindSpeedMph { get; set; } = [];

    [JsonPropertyName("wind_gusts_10m")]
    public List<double?> WindGustsMph { get; set; } = [];

    [JsonPropertyName("precipitation")]
    public List<double?> PrecipitationIn { get; set; } = [];

    // Only populated when this DTO is used for the forecast API's `past_days` gap-fill
    // call -- the archive/reanalysis endpoint doesn't expose uv_index at all.
    [JsonPropertyName("uv_index")]
    public List<double?> UvIndex { get; set; } = [];
}
