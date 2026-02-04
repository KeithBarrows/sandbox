using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sol3.Core.Extensions;
using Sol3.Data.OpenWeatherMap.Services;
using Sol3.Data.SQL.Weather;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sol3.Weather.WorkerService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("WeatherConnection");
            var pacingInMinutes = Configuration["PacingInMinutes"].ToInt();
            var cronJobWeatherDefinition = Configuration["CronJobWeatherDefinition"];
            var zipCodes = Configuration.GetSection("ZipCodes").Get<List<string>>();
            var units = Configuration.GetSection("units").Get<string>();

            // Configure your services here
            services.AddScoped<ICurrentWeatherService, CurrentWeatherService>();
            services.AddCronJob<CronJobWeather>(a => { a.CronExpression = cronJobWeatherDefinition; a.TimeZoneInfo = TimeZoneInfo.Local; a.ZipCodes = zipCodes; a.Units = units; });
            services.AddDbContext<WeatherContext>(options => options.UseSqlServer(connectionString));
        }
    }
}

/******************************************
 * https://crontab.guru/#0_0-21/3_*_*_*   *
 ******************************************/