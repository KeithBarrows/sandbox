using RestSharp;
using Sol3.Data.OpenWeatherMap.Models;
using System.Threading.Tasks;

namespace Sol3.Data.OpenWeatherMap.Services
{
    public class CurrentWeatherService : ICurrentWeatherService
    {
        private string baseUri = @"http://api.openweathermap.org/data/2.5";
        private string apiKey = "bc31521f3be3d90ef97bf1e47dd7a8e9";

        public async Task<WeatherResponse> GetByZip(string zipCode)
        {
            var client = new RestClient();
            var request = new RestRequest($"{baseUri}/weather?zip={zipCode},us&appId={apiKey}", Method.GET);
            var queryResult = await Task.Run(() => client.Execute<WeatherResponse>(request).Data);
            return queryResult;
        }
    }
}
