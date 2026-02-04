using System;
using System.Collections.Generic;

namespace Sol3.Core.Interfaces
{
    public interface IScheduleConfig<T>
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
        List<string> ZipCodes { get; set; }
        string Units { get; set; }
    }
}
