using System;
using System.Collections.Generic;
using System.Text;

namespace BigO.Extensions
{
    public static class ExtensionString
    {
        public static bool IsEmpty(this string source) => String.IsNullOrWhiteSpace(source);
    }
}
