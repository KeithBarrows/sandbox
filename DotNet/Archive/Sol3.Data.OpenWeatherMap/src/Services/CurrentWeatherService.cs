using Microsoft.Extensions.Logging;
using RestSharp;
using Sol3.Data.OpenWeatherMap.Models;
using System.Threading.Tasks;

namespace Sol3.Data.OpenWeatherMap.Services
{
    public class CurrentWeatherService : ICurrentWeatherService
    {
        private string baseUri = @"http://api.openweathermap.org/data/2.5";
        private string apiKey = "bc31521f3be3d90ef97bf1e47dd7a8e9";
        private readonly ILogger<CurrentWeatherService> _logger;

        public CurrentWeatherService() { }
        public CurrentWeatherService(ILogger<CurrentWeatherService> logger)
        {
            _logger = logger;
        }

        public async Task<WeatherResponse> GetByZipAsync(string zipCode) => await Task.Run(() => GetByZip(zipCode));
        public WeatherResponse GetByZip(string zipCode, string units = "standard")
        {
            var client = new RestClient();
            var uri = $"{baseUri}/weather?zip={zipCode},us&appId={apiKey}&units={units}";
            var request = new RestRequest(uri, Method.GET);
            var queryResult = client.Execute<WeatherResponse>(request).Data;
            return queryResult;
        }
    }
}
