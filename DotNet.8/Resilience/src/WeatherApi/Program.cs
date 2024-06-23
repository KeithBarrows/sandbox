using Polly;
using Polly.Retry;
using WeatherApi;

/**
 ** https://www.youtube.com/watch?v=kNzssE7Ir60
 **/

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<WeatherService>().AddStandardResilienceHandler();
builder.Services.AddSingleton<WeatherService>();

builder.Services.AddResiliencePipeline("default", x =>
{
    x.AddRetry(new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<Exception>(),
            Delay = TimeSpan.FromSeconds(2),
            MaxRetryAttempts = 2,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true
        })
        .AddTimeout(TimeSpan.FromSeconds(30));
});

var app = builder.Build();

app.MapGet("/weather/{city}",
    async (string city, WeatherService weatherService) =>
    {
        var weather = await weatherService.GetCurrentWeatherAsync(city);
        return weather is null ? Results.NotFound() : Results.Ok(weather);
    });

app.Run();
