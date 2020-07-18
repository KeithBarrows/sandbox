using BigO.Extensions;
using BigO.Extensions;
using BigO.Interfaces;
using BigO.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BigO.Core
{
    public class SearchListLinq : ISearch
    {
        private List<Route> _testPattern;
        public SearchListLinq(List<Route> testPattern) { _testPattern = testPattern; }
        public IEnumerable<string> Execute(IEnumerable<string> paths)
        {
            var endpoints = new List<string>();
            foreach (var path in paths)
            {
                endpoints.Add(Execute(path));
            }
            return endpoints;
        }
        public string Execute(string path)
        {
            var match = _testPattern.FirstOrDefault(x => x.Path.Equals(path, StringComparison.InvariantCultureIgnoreCase));
            if (match != null)
                return match.Endpoint;
            return "Not Found";
        }
    }

    public class SearchDictionaryLinq : ISearch
    {
        private Dictionary<string, Route> _testPattern;
        public SearchDictionaryLinq(Dictionary<string, Route> testPattern) { _testPattern = testPattern; }
        public string Execute(string path)
        {
            var r = _testPattern.FirstOrDefault(a => a.Key.Equals(path, StringComparison.InvariantCultureIgnoreCase));
            if(r.Value!= null)
                return r.Value.Endpoint;
            return "Not Found";
        }

        public IEnumerable<string> Execute(IEnumerable<string> paths)
        {
            string[] endpoints = new string[paths.Count()];
            int i = 0;
            foreach (var path in paths)
            {
                endpoints[i++] = Execute(path);
            }
            return endpoints;
        }
    }

    public class SearchList : ISearch
    {
        private List<Route> _testPattern;
        public SearchList(List<Route> testPattern) { _testPattern = testPattern; }
        public IEnumerable<string> Execute(IEnumerable<string> paths)
        {
            var endpoints = new List<string>();
            foreach (var path in paths)
            {
                endpoints.Add(Execute(path));
            }
            return endpoints;
        }
        public string Execute(string path)
        {
            var index = BinarySearchRegularPatterns(0, _testPattern.Count - 1, path);
            if (index >= 0)
                return _testPattern[index].Endpoint;
            return "Not Found";
        }

        private int BinarySearchRegularPatterns(int left, int right, string target)
        {
            int half = (right - left) / 2;
            int index = left + half;
            string checkString;

            if (half == 0)
            {
                for (int i = left; i <= right; i++)
                {
                    checkString = _testPattern[i].Path;
                    if (String.Compare(target, checkString, true) == 0)
                        return index;
                }
            }

            checkString = _testPattern[index].Path;
            if (half > 0)
            {
                if (String.Compare(target, checkString, true) == 0)
                    return index;
                if (String.Compare(target, checkString, true) < 0)
                    return BinarySearchRegularPatterns(left, half, target);
                if (String.Compare(target, checkString, true) > 0)
                    return BinarySearchRegularPatterns(left + half, right, target);
            }
            return -1;
        }
    }

    public class SearchDictionary : ISearch
    {
        private Dictionary<string, Route> _testPattern;
        public SearchDictionary(Dictionary<string, Route> testPattern) { _testPattern = testPattern; }
        public string Execute(string path)
        {
            Route route;
            // check fully defined routes first...
            if(_testPattern.TryGetValue(path, out route))      //.FirstOrDefault(a => a.Key.Equals(path, StringComparison.InvariantCultureIgnoreCase));
                return route.Endpoint;
            return "Not Found";
        }

        public IEnumerable<string> Execute(IEnumerable<string> paths)
        {
            string[] endpoints = new string[paths.Count()];
            int i = 0;
            foreach (var path in paths)
            {
                endpoints[i++] = Execute(path);
            }
            return endpoints;
        }
    }

    public class SearchTrie : ISearch
    {
        private Dictionary<string, Route2> _testPattern;
        public SearchTrie(Dictionary<string, Route2> testPattern) { _testPattern = testPattern; }
        public string Execute(string path)
        {
            var parts = path.Split('/');
            while (parts[0] == "")
                parts = parts.RemoveAt(0);
            Route2 root;
            _testPattern.TryGetValue("/", out root);
            if (root == null)
                return "404";
            if (parts.Length == 0)
                return root.Endpoint;
            var search1 = RecursePath(_testPattern.Children("/"), parts);
            var search2 = RecursePath(_testPattern.Children("/"), parts, true);
            if (search1 != "404")
                return search1;
            return search2;
        }
        private string RecursePath(Dictionary<string, Route2> routeTable, string[] parts, bool useWildcard = false)
        {
            var path = parts[0];
            parts = parts.RemoveAt(0);
            Route2 route;
            routeTable.TryGetValue(path, out route);
            if (route == null)
                return "404";
            if (parts.Length == 0)
                return route.Endpoint ?? "404";

            if (useWildcard && route.IsWildcard)
            {

            }

            return RecursePath(route.Children, parts) ?? "404";
        }

        public IEnumerable<string> Execute(IEnumerable<string> paths)
        {
            string[] endpoints = new string[paths.Count()];
            int i = 0;
            foreach (var path in paths)
            {
                endpoints[i++] = Execute(path);
            }
            return endpoints;
        }
    }
}




//using BigO.Interfaces;
//using BigO.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace BigO.Core
//{
//    // .FirstOrDefault(predicate), .Where(predicate), .ToList() and .Count() are all O(N).
//    // dictionary[key], .TryGetValue(), and.ContainsKey() are all O(1).

//    public class SearchListLinq : ISearch
//    {
//        private readonly List<Route> _regularPatternsToTestWith;
//        private readonly List<Route> _wildcardPatternsToTestWith;

//        public SearchListLinq(List<Route> regularRoutes, List<Route> wildcardRoutes)
//        {
//            _regularPatternsToTestWith = regularRoutes;
//            _wildcardPatternsToTestWith = wildcardRoutes;
//        }

//        public IEnumerable<string> Execute(IEnumerable<string> paths)
//        {
//            var endpoints = new List<string>();
//            foreach (var path in paths)
//            {
//                endpoints.Add(Execute(path));
//            }
//            return endpoints;
//        }
//        public string Execute(string path)
//        {
//            // check fully defined routes first...
//            var match = _regularPatternsToTestWith.FirstOrDefault(x => x.Path.Equals(path, StringComparison.InvariantCultureIgnoreCase));
//            if (match != null)
//                return match.Endpoint;

//            //// check wildcard routes...
//            //var source = path.Split('/').ToList();
//            //var wildcardRoutes = _wildcardPatternsToTestWith.Where(a => a.Path.Contains("/X") && a.PathParts.Length == source.Count).ToList();
//            //for (int i = 0; i < source.Count; i++)
//            //{
//            //    var target = wildcardRoutes.Where(a => a.PathParts[i] == source[i] || a.PathParts[i] == "X").ToList();
//            //    wildcardRoutes = target;
//            //}
//            //if (wildcardRoutes.Count() >= 1)
//            //    return wildcardRoutes.First().Endpoint;

//            // No routes found...
//            return "404";
//        }
//    }

//    public class SearchDictionaryLinq : ISearch
//    {
//        private Dictionary<string, Route> _regularPatternsToTestWith;
//        private Dictionary<string, Route> _wildcardPatternsToTestWith;

//        public SearchDictionaryLinq(Dictionary<string, Route> regularRoutes, Dictionary<string, Route> wildcardRoutes)
//        {
//            _regularPatternsToTestWith = regularRoutes;
//            _wildcardPatternsToTestWith = wildcardRoutes;
//        }

//        public string Execute(string path)
//        {
//            // check fully defined routes first...
//            var r = _regularPatternsToTestWith.FirstOrDefault(a => a.Key.Equals(path, StringComparison.InvariantCultureIgnoreCase));
//            if (r.Value != null)
//                return r.Value.Endpoint;

//            //// check wildcard routes...
//            //var source = path.Split('/').ToList();
//            //var wildcardRoutes = _wildcardPatternsToTestWith.Where(a => a.Value.PathParts.Length == source.Count).ToDictionary(x => x.Key, x => x.Value);
//            //for (int i = 0; i < source.Count; i++)
//            //{
//            //    var target = wildcardRoutes.Where(a => a.Value.PathParts[i] == source[i] || a.Value.PathParts[i] == "X").ToDictionary(x => x.Key, x => x.Value);
//            //    wildcardRoutes = target;
//            //}
//            //if (wildcardRoutes.Count() >= 1)
//            //    return wildcardRoutes.First().Value.Endpoint;

//            // No routes found...
//            return "404";
//        }

//        public IEnumerable<string> Execute(IEnumerable<string> paths)
//        {
//            string[] endpoints = new string[paths.Count()];
//            int i = 0;
//            foreach (var path in paths)
//            {
//                endpoints[i++] = Execute(path);
//            }
//            return endpoints;
//        }
//    }

//    public class SearchList : ISearch
//    {
//        private readonly List<Route> _regularPatternsToTestWith;
//        private readonly List<Route> _wildcardPatternsToTestWith;

//        public SearchList(List<Route> regularRoutes, List<Route> wildcardRoutes)
//        {
//            _regularPatternsToTestWith = regularRoutes;
//            _wildcardPatternsToTestWith = wildcardRoutes;
//        }

//        public IEnumerable<string> Execute(IEnumerable<string> paths)
//        {
//            var endpoints = new List<string>();
//            foreach (var path in paths)
//            {
//                endpoints.Add(Execute(path));
//            }
//            return endpoints;
//        }
//        public string Execute(string path)
//        {
//            // check fully defined routes first...
//            var index = BinarySearchRegularPatterns(0, _regularPatternsToTestWith.Count - 1, path);
//            if (index >= 0)
//                return _regularPatternsToTestWith[index].Endpoint;

//            //// check wildcard routes...
//            //var source = path.Split('/').ToList();
//            //var wildcardRoutes = _wildcardPatternsToTestWith.Where(a => a.Path.Contains("/X") && a.PathParts.Length == source.Count).ToList();
//            //for (int i = 0; i < source.Count; i++)
//            //{
//            //    var target = wildcardRoutes.Where(a => a.PathParts[i] == source[i] || a.PathParts[i] == "X").ToList();
//            //    wildcardRoutes = target;
//            //}
//            //if (wildcardRoutes.Count() >= 1)
//            //    return wildcardRoutes.First().Endpoint;

//            // No routes found...
//            return "404";
//        }

//        private int BinarySearchRegularPatterns(int left, int right, string target)
//        {
//            int half = (right - left) / 2;
//            int index = left + half;
//            var checkString = _regularPatternsToTestWith[index].Path;

//            if (half == 0)
//            {
//                for (int i = left; i <= right; i++)
//                {
//                    checkString = _regularPatternsToTestWith[i].Path;
//                    if (String.Compare(target, checkString, true) == 0)
//                        return index;
//                }
//            }
//            if (half > 0)
//            {
//                if (String.Compare(target, checkString, true) == 0)
//                    return index;
//                if (String.Compare(target, checkString, true) < 0)
//                    return BinarySearchRegularPatterns(left, half, target);
//                if (String.Compare(target, checkString, true) > 0)
//                    return BinarySearchRegularPatterns(left + half, right, target);
//            }
//            return -1;
//        }
//    }

//    public class SearchDictionary : ISearch
//    {
//        private Dictionary<string, Route> _regularPatternsToTestWith;
//        private Dictionary<string, Route> _wildcardPatternsToTestWith;

//        public SearchDictionary(Dictionary<string, Route> regularRoutes, Dictionary<string, Route> wildcardRoutes)
//        {
//            _regularPatternsToTestWith = regularRoutes;
//            _wildcardPatternsToTestWith = wildcardRoutes;
//        }

//        public string Execute(string path)
//        {
//            Route route;
//            // check fully defined routes first...
//            if (_regularPatternsToTestWith.TryGetValue(path, out route))      //.FirstOrDefault(a => a.Key.Equals(path, StringComparison.InvariantCultureIgnoreCase));
//                return route.Endpoint;

//            //// check wildcard routes...
//            //var source = path.Split('/').ToList();
//            //var wildcardRoutes = _wildcardPatternsToTestWith.Where(a => a.Value.PathParts.Length == source.Count).ToDictionary(x => x.Key, x => x.Value);
//            //for (int i = 0; i < source.Count; i++)
//            //{
//            //    var target = wildcardRoutes.Where(a => a.Value.PathParts[i] == source[i] || a.Value.PathParts[i] == "X").ToDictionary(x => x.Key, x => x.Value);
//            //    wildcardRoutes = target;
//            //}
//            //if (wildcardRoutes.Count() >= 1)
//            //    return wildcardRoutes.First().Value.Endpoint;

//            // No routes found...
//            return "404";
//        }

//        public IEnumerable<string> Execute(IEnumerable<string> paths)
//        {
//            string[] endpoints = new string[paths.Count()];
//            int i = 0;
//            foreach (var path in paths)
//            {
//                endpoints[i++] = Execute(path);
//            }
//            return endpoints;
//        }
//    }
//}
