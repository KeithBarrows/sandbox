using Sol3.Data.OpenWeatherMap.Models;
using System.Threading.Tasks;

namespace Sol3.Data.OpenWeatherMap.Services
{
    public interface ICurrentWeatherService
    {
        Task<WeatherResponse> GetByZip(string zipCode);
    }
}