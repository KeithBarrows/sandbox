using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sol3.Infrastructure.Configuration;
using Sol3.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

namespace Sol3.Infrastructure.Logging
{
    public static class SerilogSetup
    {
        public static Serilog.ILogger Setup(string appName)
        {
            AppConfiguration = ConfigurationManager.InitializeConfiguration();

            if (CanSelfLog)
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"{appName}_{environmentName}_SelfLog.log");
                var file = File.CreateText(path);
                Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(file));
                Serilog.Debugging.SelfLog.WriteLine("Setting up Serilog");
            }

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", AppConfiguration["Serilog:Overrides:Microsoft"].LogLevel(LogEventLevel.Information))
                .MinimumLevel.Override("System", AppConfiguration["Serilog:Overrides:System"].LogLevel(LogEventLevel.Information))
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", appName)
                .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

            return InternalSetup(loggerConfiguration, appName);
        }

        public static Serilog.ILogger Setup(IDictionary<string,string> enrichWithProperties)
        {
            AppConfiguration = ConfigurationManager.InitializeConfiguration();

            var appName = "UNKNOWN APP.  PASS [appName] IN PROGRAM.CS.";
            if (enrichWithProperties.ContainsKey("appName"))
                appName = enrichWithProperties.FirstOrDefault(kvp => kvp.Key.Equals("appName", StringComparison.InvariantCultureIgnoreCase)).Value;

            if (CanSelfLog)
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"{appName}_{environmentName}_SelfLog.log");
                var file = File.CreateText(path);
                Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(file));
                Serilog.Debugging.SelfLog.WriteLine("Setting up Serilog");
            }

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", AppConfiguration["Serilog:Overrides:Microsoft"].LogLevel(LogEventLevel.Information))
                .MinimumLevel.Override("System", AppConfiguration["Serilog:Overrides:System"].LogLevel(LogEventLevel.Information))
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", appName)
                .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

            loggerConfiguration = enrichWithProperties.Aggregate(loggerConfiguration, (current, kvp) => current.Enrich.WithProperty(kvp.Key, kvp.Value));

            return InternalSetup(loggerConfiguration, appName);
        }

        private static Serilog.ILogger InternalSetup(LoggerConfiguration loggerConfiguration, string appName)
        {
            if (IsActiveSeq)
            {
                var url = AppConfiguration["Serilog:Seq:Url"];
                loggerConfiguration = loggerConfiguration.WriteTo.Seq(url, RestrictedToMinimumLevelSeq);

                if (CanSelfLog)
                    Serilog.Debugging.SelfLog.WriteLine("Adding SEQ Sink, url={0}, RestrictedToMinimumLevel={1}", url, RestrictedToMinimumLevelSeq);
            }

            if (IsActiveConsole)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Literate, restrictedToMinimumLevel: RestrictedToMinimumLevelConsole);

                if (CanSelfLog)
                    Serilog.Debugging.SelfLog.WriteLine("Adding Console Sink, RestrictedToMinimumLevel={0}", RestrictedToMinimumLevelConsole);
            }

            if (IsActiveMsSql)
            {
                var connString = AppConfiguration["Serilog:MSSQL:ConnectionString"];
                var tableName = AppConfiguration["Serilog:MSSQL:TableName"];
                var autoCreateTable = AppConfiguration["Serilog:MSSQL:autoCreateSqlTable"].ToBool(false);
                loggerConfiguration = loggerConfiguration.WriteTo.MSSqlServer(connectionString: connString
                    , tableName: tableName
                    , restrictedToMinimumLevel: RestrictedToMinimumLevelMsSql
                    , autoCreateSqlTable: autoCreateTable);

                if (CanSelfLog)
                {
                    var msg = $"Adding MSSQL Sink, connectionString={connString}, tableName={tableName}, RestrictedToMinimumLevel={RestrictedToMinimumLevelConsole}, autoCreateSqlTable={autoCreateTable}";
                    Serilog.Debugging.SelfLog.WriteLine(msg);
                }
            }

            if (IsActiveFile)
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"{appName.Replace(" ", "_")}_{environmentName}");
                var logFile = path + "_{Date}.log";
                loggerConfiguration = loggerConfiguration.WriteTo.RollingFile(new JsonFormatter(), logFile, RestrictedToMinimumLevelFile);

                if (CanSelfLog)
                    Serilog.Debugging.SelfLog.WriteLine("Adding RollingFile Sink, LogFile={0}, RestrictedToMinimumLevel={1}", logFile, RestrictedToMinimumLevelConsole);
            }

            return loggerConfiguration.CreateLogger();
        }

        private static IConfigurationRoot AppConfiguration { get; set; }
        private static bool IsActiveSeq => AppConfiguration["Serilog:Seq:IsActive"].ToBool(false) && AppConfiguration["Serilog:Seq:Url"].Length > 0;
        private static bool IsActiveConsole => AppConfiguration["Serilog:Console:IsActive"].ToBool(false);
        private static bool IsActiveMsSql => AppConfiguration["Serilog:MSSQL:IsActive"].ToBool(false) && AppConfiguration["Serilog:MSSQL:ConnectionString"].Length > 0 && AppConfiguration["Serilog:MSSQL:TableName"].Length > 0;
        private static bool IsActiveFile => AppConfiguration["Serilog:File:IsActive"].ToBool(false);
        private static bool CanSelfLog => AppConfiguration["Serilog:SelfLog"].ToBool(false);
        
        private static LogEventLevel RestrictedToMinimumLevelSeq => AppConfiguration["Serilog:Seq:restrictedToMinimumLevel"].LogLevel();
        private static LogEventLevel RestrictedToMinimumLevelConsole => AppConfiguration["Serilog:Console:restrictedToMinimumLevel"].LogLevel();
        private static LogEventLevel RestrictedToMinimumLevelMsSql => AppConfiguration["Serilog:MSSQL:restrictedToMinimumLevel"].LogLevel();
        private static LogEventLevel RestrictedToMinimumLevelFile => AppConfiguration["Serilog:File:restrictedToMinimumLevel"].LogLevel();

        private static LogEventLevel LogLevel(this string logLevel, LogEventLevel defaultEventLevel = LogEventLevel.Error)
        {
            // If we got a number, cast it and return...
            if (int.TryParse(logLevel, out var test))
            {
                if(test < 0 || test > 5)
                    throw new ArgumentOutOfRangeException(logLevel, "LogLevel must be an integer between 0 and 5!");
                return (LogEventLevel)test;
            }

            // If we got text, let's try to get a value here...
            switch (logLevel.ToUpper())
            {
                case "FATAL":
                case "CRITICAL":
                    return LogEventLevel.Fatal;
                case "ERR":
                case "ERROR":
                    return LogEventLevel.Error;
                case "WARN":
                case "WARNING":
                    return LogEventLevel.Warning;
                default:
                    return LogEventLevel.Information;
                case "DEBUG":
                    return LogEventLevel.Debug;
                case "VERBOSE":
                case "TRACE":
                    return LogEventLevel.Verbose;
            }
        }
    }
}
