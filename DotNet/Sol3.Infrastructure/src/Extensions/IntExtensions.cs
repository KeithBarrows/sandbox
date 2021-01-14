using System.Collections.Generic;
using System.Linq;

namespace Sol3.Infrastructure.Extensions
{
    public static class IntExtensions
    {
        public static bool Between(this int src, int lower, int upper)
        {
            return src >= lower && src <= upper;
        }

        public static bool ContainedIn(this int src, Dictionary<int, string> allowed)
        {
            return allowed.ContainsKey(src);
        }
        public static string AllowedText(this int src, Dictionary<int, string> allowed)
        {
            string ret;
            return allowed.TryGetValue(src, out ret) ? ret : string.Empty;
        }
        public static string ClassName<T>(this T src) where T : class, new()
        {
            var fullName = src.ToString().Split('.');
            return fullName.ToList().Last();
        }
        public static bool ToBool(this int src)
        {
            return src != 0;
        }
    }
}