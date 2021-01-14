using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Sol3.Infrastructure.Extensions;

namespace Sol3.Infrastructure.Configuration
{
    public static class ConfigurationManager
    {
        public static IConfigurationRoot InitializeConfiguration<T>() where T : class
        {
            var dir = Directory.GetCurrentDirectory();

            var globalbuilder = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile("globalconfig.json");
            var globalConfiguration = globalbuilder.Build();

            var stagingEnvironment = globalConfiguration["StagingEnvironment"] ?? "development";
            var hasAppSettings = globalConfiguration["HasAppSettings"].ToBool();

            var builder = new ConfigurationBuilder();

            if(hasAppSettings)
                builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{stagingEnvironment}.json", optional: true, reloadOnChange: true);

            if (stagingEnvironment.Equals("Development", StringComparison.CurrentCultureIgnoreCase))
                builder.AddUserSecrets<T>();

            var configuration = builder.Build();
            
            return configuration;
        }
        public static IConfigurationRoot InitializeConfiguration()
        {
            var dir = Directory.GetCurrentDirectory();

            var globalbuilder = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile("globalconfig.json");
            var globalConfiguration = globalbuilder.Build();

            var stagingEnvironment = globalConfiguration["StagingEnvironment"] ?? "development";
            var hasAppSettings = globalConfiguration["HasAppSettings"].ToBool();

            var builder = new ConfigurationBuilder();

            if(hasAppSettings)
                builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{stagingEnvironment}.json", optional: true, reloadOnChange: true);

            // if (stagingEnvironment.Equals("Development", StringComparison.CurrentCultureIgnoreCase))
            //     builder.AddUserSecrets();

            var configuration = builder.Build();
            
            return configuration;
        }
    }
}
