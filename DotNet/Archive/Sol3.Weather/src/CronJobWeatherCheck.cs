using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sol3.Data.OpenWeatherMap.Models;
using Sol3.Data.OpenWeatherMap.Services;

namespace Sol3.Weather
{
    public class CronJobWeatherCheck
    {
        private readonly ICurrentWeatherService _currentWeatherService;

        public CronJobWeatherCheck(ICurrentWeatherService currentWeatherService)
        {
            _currentWeatherService = currentWeatherService;
        }

        [FunctionName("CronJobWeatherCheck")]
        public async void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            //log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now} - {myTimer.Schedule.ToString()}");

            //var zipCodes = new[] {"32958", "32720", "33067", "65355", "65301" };
            //var response = new List<WeatherResponse>();
            //foreach(var zip in zipCodes)
            //    response.Add(await _currentWeatherService.GetByZip(zip));

            //_ = response.Count;
        }
    }
}
