using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sol3.Core.Extensions
{
    public static class VariableExtensions
    {
        public static int ToInt(this string src)
        {
            if (int.TryParse(src, out int result))
                return result;
            return 0;
        }
    }
}
