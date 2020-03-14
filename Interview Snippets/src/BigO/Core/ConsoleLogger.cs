using BigO.Interfaces;
using System;

namespace BigO.Core
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Log(Exception ex, string message)
        {
            Console.WriteLine($"{ex.Message}{Environment.NewLine}  -- {message}");
        }
    }
}
