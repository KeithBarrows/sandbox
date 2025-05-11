using WeatherApi;
using WeatherApi.Startup;
using WeatherApi.Features.Weather;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;

/**
 ** https://www.youtube.com/watch?v=kNzssE7Ir60
 **/

var builder = WebApplication.CreateBuilder(args);

// Configure HTTP client with standard resilience handler
builder.Services.AddHttpClient<WeatherService>().AddStandardResilienceHandler();
// Add resilience enrichment for better telemetry
builder.Services.AddResilienceEnricher();
builder.Services.AddSingleton<WeatherService>();

var app = builder.Build();
app.MapWeatherEndpoints();
app.Run();


/******************************************************************************************************
 * https://learn.microsoft.com/en-us/dotnet/core/resilience/?tabs=dotnet-cli
 * 
 ******************************************************************************************************/