using Sol3.Data.OpenWeatherMap.Models;
using Sol3.Data.OpenWeatherMap.Services;
using System.Collections.Generic;

namespace Sol3.WeatherCore
{
    class Program
    {
        static void Main(string[] args)
        {
            ICurrentWeatherService currentWeatherService = new CurrentWeatherService();

            var zipCodes = new[] { "32958", "32720", "33067", "65355", "65301" };
            var response = new List<WeatherResponse>();
            foreach (var zip in zipCodes)
                response.Add(currentWeatherService.GetByZip(zip));

            _ = response.Count;
        }
    }
}
