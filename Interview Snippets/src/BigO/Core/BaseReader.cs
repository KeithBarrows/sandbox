using BigO.Extensions;
using BigO.Extensions;
using BigO.Interfaces;
using BigO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BigO.Core
{
    public abstract class BaseReader : IReader
    {
        public BaseReader()
        {
            _logger = new ConsoleLogger();
        }
        public BaseReader(ILogger logger)
        {
            _logger = logger;
        }

        private ILogger _logger;
        public ILogger Logger => _logger;

        public abstract List<Route> ReadConfig();
        public abstract List<string> ReadTargets();
        public Dictionary<string, Route2> CreateRouteTable(List<Route> routes)
        {
            var routeTable = new Dictionary<string, Route2>();
            routes.ForEach(route => RecursePathAddToDictionary(route, ref routeTable));
            return routeTable;
        }
        private void RecursePathAddToDictionary(Route rawRoute, ref Dictionary<string, Route2> routeTable)
        {
            var parts = rawRoute.Path.Split('/');

            // strip any leading blanks...
            while (parts.Length > 0 && String.IsNullOrWhiteSpace(parts[0]))
            {
                parts = parts.RemoveAt(0);
            }

            // handle root route...
            if(parts.Length <= 0 && rawRoute.Path.Equals("/") && routeTable.Count <= 0)
            {
                // add root...
                var rootRoute = new Route2 { Path = "/", Endpoint = rawRoute.Endpoint, Parent = null, WildcardCount = 0 };
                routeTable.Add(rawRoute.Path, rootRoute);
                return;
            }

            // handle children of root...
            var children = routeTable.Children("/");
            if (children != null)
            {
                AddUpdateChild(ref children, parts, rawRoute.Endpoint, rawRoute.Path);
            }
        }

        private void AddUpdateChild(ref Dictionary<string, Route2> routeTable, string[] parts, string endPoint, string fullPath)
        {
            var path = parts[0];        // get this node
            parts = parts.RemoveAt(0);  // strip it from the array

            // is it in the dictionary?
            Route2 thisRoute;
            if (!routeTable.TryGetValue(path, out thisRoute))
            {
                thisRoute = new Route2 { Path = path, WildcardCount = path == "X" ? 1 : 0, Endpoint = parts.Length == 0 ? endPoint : string.Empty };
            }

            if(parts.Length > 0)
            {
                // we still need to iterate deeper on the path...
                var children = thisRoute.Children;
                AddUpdateChild(ref children, parts, endPoint, fullPath);
            }
            else
            {
                // end of the path...
                if (!thisRoute.Endpoint.Equals(endPoint, StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception($"You cannot redfine an endPoint definition during initial load!  [{endPoint} for {fullPath}]");
                if (thisRoute.Endpoint.IsEmpty())
                    thisRoute.Endpoint = endPoint;
            }

            if (routeTable.TryAdd(path, thisRoute))
                return;
        }
    }

    public class TestReader : BaseReader
    {
        public TestReader(ILogger logger) : base(logger) { }
        public override List<Route> ReadConfig()
        {
            //Logger.Log("Endpoint configurations are:");
            List<Route> routes = new List<Route>();
            routes.Add(new Route { Path = "/", Endpoint = "rootEndpoint" });
            routes.Add(new Route { Path = "/find", Endpoint = "findRootEndpoint" });
            routes.Add(new Route { Path = "/find/friends", Endpoint = "findFriendsEndpoint" });
            routes.Add(new Route { Path = "/find/lists", Endpoint = "findListsEndpoint" });
            routes.Add(new Route { Path = "/find/X", Endpoint = "findEndpoint" });
            routes.Add(new Route { Path = "/find/X/friends", Endpoint = "findFriendsEndpoint" });
            routes.Add(new Route { Path = "/find/X/lists", Endpoint = "findListsEndpoint" });
            routes.Add(new Route { Path = "/find/X/lists/X", Endpoint = "findListIdEndpoint" });
            routes.Add(new Route { Path = "/X/friends", Endpoint = "findFriendsEndpoint" });
            routes.Add(new Route { Path = "/X/lists", Endpoint = "findListsEndpoint" });
            routes.Add(new Route { Path = "/settings", Endpoint = "settingsEndpoint" });
            routes.ForEach(a => Logger.Log($"{a.Path}\t{a.Endpoint}"));
            return routes;
        }
        public override List<string> ReadTargets()
        {
            Logger.Log("");
            Logger.Log("Paths to be matched are:");
            List<string> paths = new List<string>();
            //paths.Add("/find/friends/friends");
            //paths.Add("/abc/lists");
            //paths.Add("/");
            //paths.Add("/find");
            //paths.Add("/find/friends");
            //paths.Add("/find/123");
            //paths.Add("/find/123/friends");
            //paths.Add("/find/123/friends/zzz");
            //paths.Add("/settings");
            //paths.Add("/aaa/bbb");
            paths.Add("/aa/aa/aa");
            paths.Add("/aa/aa/nn");
            paths.Add("/aa/bb/aa");
            paths.Add("/aa/bb/nn");
            paths.Add("/aa/cc/aa");
            paths.ForEach(a => Logger.Log(a));
            return paths;
        }
    }

    public class LineReader : BaseReader
    {
        public LineReader(ILogger logger) : base(logger) { }
        public override List<Route> ReadConfig()
        {
            string line;
            List<Route> routes = new List<Route>();
            Logger.Log("Enter your endpoint configurations one line at a time.  When finished, just hit enter on a blank line.");
            while ((line = Console.ReadLine()) != null && line.Length != 0)
            {
                string[] tokenizedLine = line.Split(' ');
                routes.Add(new Route { Path = tokenizedLine[0], Endpoint = tokenizedLine[1] });
            }
            routes.ForEach(a => Logger.Log($"{a.Path}\t{a.Endpoint}"));
            return routes;
        }

        public override List<string> ReadTargets()
        {
            string line;
            var paths = new List<string>();
            Logger.Log("");
            Logger.Log("Enter multiple paths to be matched, one line at a time.  When finished, just hit enter on a blank line.");
            while ((line = Console.ReadLine()) != null && line.Length != 0)
            {
                paths.Add(line);
            }
            return paths;
        }
    }

    public class FileReader : BaseReader
    {
        private string _routePath;
        private string _sourcePath;

        public FileReader(string[] args, ILogger logger) : base(logger)
        {
            _routePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]);
            _sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[1]);
        }

        public override List<Route> ReadConfig()
        {
            Logger.Log($"Endpoint configurations as read from file [{_routePath}] are:");
            List<Route> routes = new List<Route>();
            var source = File.ReadAllLines(_routePath).ToList();

            // remove double (or more) spaces...
            for (int i = 0; i < source.Count; i++)
                while (source[i].Contains("  "))
                    source[i] = source[i].Replace("  ", " ");

            if (source.Count > 0)
            {
                foreach (var line in source)
                {
                    string[] tokenizedLine = line.Split(' ');
                    routes.Add(new Route { Path = tokenizedLine[0], Endpoint = tokenizedLine[1] });
                }
            }
            routes.ForEach(a => Logger.Log($"{a.Path}\t{a.Endpoint}"));
            return routes;
        }

        public override List<string> ReadTargets()
        {
            Logger.Log("");
            Logger.Log($"Paths to be matched as read from file [{_sourcePath}] are:");
            List<string> paths = new List<string>();
            var source = File.ReadAllLines(_sourcePath).ToList();
            if (source.Count > 0)
            {
                source.ForEach(line => paths.Add(line));
            }
            paths.ForEach(a => Logger.Log(a));
            return paths;
        }
    }
}
