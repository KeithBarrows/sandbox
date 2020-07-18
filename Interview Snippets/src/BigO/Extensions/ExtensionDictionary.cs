using BigO.Models;
using System.Collections.Generic;

namespace BigO.Extensions
{
    public static class ExtensionDictionary
    {
        public static Dictionary<string, Route2> Children(this Dictionary<string, Route2> routeTable, string parentKey)
        {
            Route2 thisRoute;
            routeTable.TryGetValue(parentKey, out thisRoute);
            if (thisRoute == null)
                return null;
            return thisRoute.Children;
        }
        public static bool IsWildCardRoute(this Dictionary<string, Route2> routeTable)
        {
            var result = false;


            return result;
        }
    }
}
