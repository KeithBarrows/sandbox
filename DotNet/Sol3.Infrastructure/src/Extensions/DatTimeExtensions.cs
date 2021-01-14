using System;

namespace Sol3.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTimeOffset? src)
        {
            if(src.HasValue) 
                return src.Value;
                
            var today = DateTime.Now;
            return new DateTimeOffset(today.Year, today.Month, today.Day, 0, 0, 0, new TimeSpan(0,0,0));
        }
    }
}