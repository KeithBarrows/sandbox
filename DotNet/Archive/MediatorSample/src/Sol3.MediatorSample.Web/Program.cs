using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using Serilog;
using Sol3.MediatorSample.Web.Core;
using System;

namespace Sol3.MediatorSample.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(AppInfo.Configuration)
                .Enrich.WithProperty("Environment", AppInfo.EnvironmentName)
                .Enrich.WithProperty("Application", AppInfo.ApplicationName)
                .Destructure.AsScalar<JObject>()
                .Destructure.AsScalar<JArray>()
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.Information("Web host stopped");
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(AppInfo.Configuration)
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
