using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC.NewbLibrary
{
    public static class Extend
    {
        public static bool IsEmpty(this string src) => string.IsNullOrWhiteSpace(src);
        public static bool IsEqualTo(this string src, string tgt)
        {
            src = src ?? "";
            tgt = tgt ?? "";
            return src.Equals(tgt, StringComparison.OrdinalIgnoreCase);
        }
    }
}
