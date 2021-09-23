using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Sol3.Data.OpenWeatherMap.Services;
using Sol3.Infrastructure.Logging;
using Sol3.Weather;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Sol3.Weather
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var logger = SerilogSetup.Setup("Sol3.Weather");
            logger.Information("Startup!");

            //builder.Services.AddSingleton<Serilog.ILogger, logger>();
            builder.Services.AddTransient<ICurrentWeatherService, CurrentWeatherService>();
        }
    }
}
