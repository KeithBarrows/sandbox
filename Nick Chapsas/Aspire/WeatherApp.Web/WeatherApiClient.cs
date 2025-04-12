using WeatherApp.Web.Components.Pages;

namespace WeatherApp.Web;

public class WeatherApiClient
{
    private readonly HttpClient _client;

    public WeatherApiClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Weather.WeatherForecast[]> GetWeatherAsync()
    {
        return await _client.GetFromJsonAsync<Weather.WeatherForecast[]>("weatherforecast") ?? [];
    }
}
