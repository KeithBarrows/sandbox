using System.Net;
using Polly;
using Polly.Registry;
using Polly.Retry;

namespace WeatherApi;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid=661a1bcf2fae84fe5e5298071369b64c&units=metric";

        var weatherResponse = await _httpClient.GetAsync(url);
        
        if (weatherResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        return await weatherResponse.Content.ReadFromJsonAsync<WeatherResponse>();
    }
}

/*
public class WeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ResiliencePipelineProvider<string> _pipelineProvider;

    public WeatherService(IHttpClientFactory httpClientFactory, ResiliencePipelineProvider<string> pipelineProvider)
    {
        _httpClientFactory = httpClientFactory;
        _pipelineProvider = pipelineProvider;
    }

    public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid=661a1bcf2fae84fe5e5298071369b64c&units=metric";
        var httpClient = _httpClientFactory.CreateClient();

        var pipeline = _pipelineProvider.GetPipeline("default");
        
        var weatherResponse = await pipeline.ExecuteAsync(
            async ct => await httpClient.GetAsync(url, ct));
        
        if (weatherResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        return await weatherResponse.Content.ReadFromJsonAsync<WeatherResponse>();
    }
}
*/
