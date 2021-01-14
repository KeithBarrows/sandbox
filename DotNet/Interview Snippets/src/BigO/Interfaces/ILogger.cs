using System;
using System.Collections.Generic;
using System.Text;

namespace BigO.Interfaces
{
    // A very simple interface; normally I would call in the MS ILogger but we don't need that here...
    // Not using log levels or formatters...
    public interface ILogger
    {
        void Log(string message);
        void Log(Exception ex, string message);
    }
}
