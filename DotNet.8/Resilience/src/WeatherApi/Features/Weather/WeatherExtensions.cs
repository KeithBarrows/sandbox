using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WeatherApi.Features.Weather;

public static class WeatherExtensions
{
    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/weather/{city}",
            async (string city, WeatherService weatherService) =>
            {
                var weather = await weatherService.GetCurrentWeatherAsync(city);
                return weather is null ? Results.NotFound() : Results.Ok(weather);
            });

        return app;
    }
}
