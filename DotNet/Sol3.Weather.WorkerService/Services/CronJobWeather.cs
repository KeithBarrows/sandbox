using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestSharp;
using Sol3.Core.Abstracts;
using Sol3.Core.Interfaces;
using Sol3.Data.OpenWeatherMap.Models;
using Sol3.Data.OpenWeatherMap.Services;
using Sol3.Data.SQL.Weather;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using dbModels = Sol3.Data.SQL.Weather.Models;

namespace Sol3.Weather.WorkerService
{
    public class CronJobWeather : CronJobService
    {
        private readonly ILogger<CronJobWeather> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IScheduleConfig<CronJobWeather> _config;

        public CronJobWeather(IScheduleConfig<CronJobWeather> config, ILogger<CronJobWeather> logger, IServiceScopeFactory serviceScopeFactory) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _config = config;
            _logger.LogDebug("Cron Job configured as {@Config}", config);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Weather Cron Job starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Weather Cron Job is working.");

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var timeStamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                var currentWeatherService = scope.ServiceProvider.GetService<ICurrentWeatherService>();
                var currentWeatherServiceResponse = new List<WeatherResponse>();

                // TODO - call weather service...
                var zipCodes = _config.ZipCodes;
                foreach (var zip in zipCodes)
                    currentWeatherServiceResponse.Add(currentWeatherService.GetByZip(zip, _config.Units));

                //// save to DB...
                //using var db = scope.ServiceProvider.GetService<WeatherContext>();
                //{
                //    foreach (var weatherResponse in currentWeatherServiceResponse)
                //    {
                //        var dbWeatherResponse = new dbModels.WeatherResponse(weatherResponse, timeStamp);
                //        db.WeatherResponses.Add(dbWeatherResponse);
                //        _logger.LogDebug("Saving {@WeatherResponse} to database.", weatherResponse);
                //    }
                //    db.SaveChanges();
                //}

                // save to csv file...
                foreach (var weatherResponse in currentWeatherServiceResponse)
                {
                    var (name, pressure, temp, feelsLike) = weatherResponse;
                }

                //// save to CSV file...
                //var file = Path.Combine(Environment.CurrentDirectory, "Weather.csv");
                //File.OpenWrite(file);
                //if (!File.Exists(file))
                //    File.CreateText(file);


                //foreach (var weatherResponse in currentWeatherServiceResponse)
                //{

                //}

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Job} failed", nameof(CronJobService));
            }

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Weather Cron Job is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
