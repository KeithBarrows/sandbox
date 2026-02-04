using System;
using System.Collections.Generic;

namespace Sol3.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToSingleString(this string[] src)
        {
            var message = string.Empty;
            var token = string.Empty;
            foreach (var q in src)
            {
                message = $"{message}{token}{q}";
                token = Environment.NewLine;
            }
            return message;
        }

        public static bool IsNullOrEmpty(this string src)
        {
            return string.IsNullOrEmpty(src);
        }

        public static bool IsNullOrWhiteSpace(this string src)
        {
            return string.IsNullOrWhiteSpace(src);
        }

        public static bool ContainedIn(this string src, Dictionary<int, string> allowed)
        {
            return (src.IsInt() && allowed.ContainsKey(src.ToInt())) || allowed.ContainsValue(src.ToUpper());
        }

        public static int ToInt(this string src, int? @default = null)
        {
            if (int.TryParse(src, out int ret))
                return ret;
            if (@default.HasValue)
                return @default.Value;
            throw new ArgumentException($"The value passed in [{src}] cannot be converted to a valid INT and there was no default defined!");
        }

        public static bool IsInt(this string src)
        {
            return int.TryParse(src, out _);
        }
        public static string AllowedText(this string src, Dictionary<int, string> allowed)
        {
            if (src.IsInt() && allowed.ContainsKey(src.ToInt()))
            {
                if (allowed.TryGetValue(src.ToInt(), out string ret))
                    return ret;
            }
            return allowed.ContainsValue(src.ToUpper()) ? src : string.Empty;
        }

        public static bool ToBool(this string src, bool @default = false)
        {
            return bool.TryParse(src, out var ret) ? ret : @default;
        }

    }
}
