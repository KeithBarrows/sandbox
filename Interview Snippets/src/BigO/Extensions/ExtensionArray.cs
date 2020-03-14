using System;
using System.Collections.Generic;
using System.Text;

namespace BigO.Extensions
{
    public static class ExtensionArray
    {
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            List<T> tmp = new List<T>(source);
            tmp.RemoveAt(index);
            return tmp.ToArray();
        }
    }
}
