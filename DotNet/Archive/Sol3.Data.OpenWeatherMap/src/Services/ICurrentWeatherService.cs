using Sol3.Data.OpenWeatherMap.Models;
using System.Threading.Tasks;

namespace Sol3.Data.OpenWeatherMap.Services
{
    public interface ICurrentWeatherService
    {
        Task<WeatherResponse> GetByZipAsync(string zipCode);
        WeatherResponse GetByZip(string zipCode, string units);
    }
}