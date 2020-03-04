using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sol3.MediatorSample.Web.Core
{
    public class AppInfo : Singleton<AppInfo>
    {
        public static bool IsProduction => EnvironmentName.ToUpper() == "PRODUCTION" || (!IsStaging && !IsDevelopment);
        public static bool IsStaging => EnvironmentName.ToUpper() == "STAGING";
        public static bool IsDevelopment => EnvironmentName.ToUpper() == "DEVELOPMENT";
        public static string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        public static string CurrentDirectoryName => Directory.GetCurrentDirectory() ?? Environment.CurrentDirectory;
        public static string ApplicationName => Assembly.GetExecutingAssembly().GetName().Name;
        public static IConfiguration Configuration => new ConfigurationBuilder()
            .SetBasePath(CurrentDirectoryName)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
