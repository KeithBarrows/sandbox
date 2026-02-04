using Microsoft.Extensions.Hosting;
using Serilog;
using Sol3.Core.Extensions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sol3.Weather.WorkerService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                // Log.Logger will likely be internal type "Serilog.Core.Pipeline.SilentLogger".
                if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
                {
                    // Loading configuration or Serilog failed.
                    // This will create a logger that can be captured by Azure logger.
                    // To enable Azure logger, in Azure Portal:
                    // 1. Go to WebApp
                    // 2. App Service logs
                    // 3. Enable "Application Logging (Filesystem)", "Application Logging (Filesystem)" and "Detailed error messages"
                    // 4. Set Retention Period (Days) to 10 or similar value
                    // 5. Save settings
                    // 6. Under Overview, restart web app
                    // 7. Go to Log Stream and observe the logs
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .CreateLogger();
                }

                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
                                                                           .UseStartup<Startup>()
                                                                           .UseSerilog((hostingContext, loggerConfiguration) =>
                                                                           {
                                                                               loggerConfiguration
                                                                                   .ReadFrom.Configuration(hostingContext.Configuration)
                                                                                   .Enrich.FromLogContext()
                                                                                   .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                                                                                   .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);
                                                                           });
    }
}


/************************************************************************************************************
 * https://stackoverflow.com/questions/41407221/startup-cs-in-a-self-hosted-net-core-console-application    *
 * https://github.com/sonicmouse/Host.CreateDefaultBuilder.Example                                          *
 * -------------------------------------------------------------------------------------------------------- *
 * https://jkdev.me/asp-net-core-serilog/                                                                   *
 ************************************************************************************************************/