using System;

namespace Sol3.Infrastructure.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToFullString(this Exception src)
        {
            var ex = src;
            var msg = string.Empty;
            var token = string.Empty;
            while (ex != null)
            {
                msg = $"{msg}{token}{ex.Message}";
                token = Environment.NewLine;
                ex = ex.InnerException;
            }

            msg = $"{msg}{token}{token}{src.StackTrace}";
            return msg;
        }
    }
}