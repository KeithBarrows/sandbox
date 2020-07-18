using BigO.Models;
using System.Collections.Generic;

namespace BigO.Interfaces
{
    interface IReader
    {
        ILogger Logger { get; }
        List<Route> ReadConfig();
        List<string> ReadTargets();
        Dictionary<string, Route2> CreateRouteTable(List<Route> routes);
    }
}
