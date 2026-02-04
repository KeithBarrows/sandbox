//using System;
//using System.IO;
//using Microsoft.Extensions.Configuration;
//using Sol3.Infrastructure.Extensions;

//namespace Sol3.Infrastructure.Configuration
//{
//    public static class ConfigurationManager
//    {
//        public static IConfigurationRoot InitializeConfiguration<T>() where T : class
//        {
//            InitialBuilderSetup();

//            if (StagingEnvironment.Equals("Development", StringComparison.CurrentCultureIgnoreCase))
//                Builder.AddUserSecrets<T>();

//            Configuration = Builder.Build();
            
//            return Configuration;
//        }
//        public static IConfigurationRoot InitializeConfiguration()
//        {
//            InitialBuilderSetup();

//            Configuration = Builder.Build();
            
//            return Configuration;
//        }
//        private static void InitialBuilderSetup()
//        {
//            var dir = Directory.GetCurrentDirectory();

//            var globalbuilder = new ConfigurationBuilder()
//                .SetBasePath(dir)
//                .AddJsonFile("globalconfig.json");
//            var globalConfiguration = globalbuilder.Build();

//            StagingEnvironment = globalConfiguration["StagingEnvironment"] ?? "development";
//            HasAppSettings = globalConfiguration["HasAppSettings"].ToBool();

//            Builder = new ConfigurationBuilder();

//            if (HasAppSettings)
//                Builder.SetBasePath(Directory.GetCurrentDirectory())
//                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                    .AddJsonFile($"appsettings.{StagingEnvironment}.json", optional: true, reloadOnChange: true);
//        }

//        private static string StagingEnvironment { get; set; }
//        private static bool HasAppSettings { get; set; }
//        private static ConfigurationBuilder Builder { get; set; }
//        private static IConfigurationRoot Configuration { get; set; }
//    }
//}
