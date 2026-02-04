using Sol3.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Sol3.Core.Models
{
    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
        public List<string> ZipCodes { get; set; }
        public string Units { get; set; }
    }
}
