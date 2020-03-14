//#define RECURSIVE
//#define TEST

//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace RouteDispatch
//{
//    public class Program2
//    {
//        private const string EndpointKey = "/";

//        public static void Main2(string[] args)
//        {
//#if !TEST
//            Run(ReadConfig(args[1]), ReadPaths()).ForEach(Console.WriteLine);
//#else
//			string[] routes =
//			{
//				"/abc/cdef endpoint1",
//				"/abc/ghij endpoint2",
//				"/abc/X endpoint3",
//				"/xyz/tuvw endpoint4",
//			};
//			string[] paths =
//			{
//				"/abc/cdef",
//				"/abc/klmn",
//				"/xyz/tuvw",
//			};
//			Run(routes, paths).ForEach(Console.WriteLine);
//#endif
//        }

//        public static IEnumerable<string> Run(IEnumerable<string> config, IEnumerable<string> paths)
//        {
//            var routes = ParseRoutes(config); // time: O(n * m) space: O(n * m) 

//            foreach (var path in paths)
//            {
//                var endpoint = Dispatch(routes, path); // time: O(m) space: O(m)
//                yield return endpoint ?? "404";
//            }
//        }

//#if !TEST
//        private static IEnumerable<string> ReadConfig(string filename)
//        {
//            return File.ReadAllLines(filename);
//        }

//        private static IEnumerable<string> ReadPaths()
//        {
//            string line;
//            while ((line = Console.ReadLine()) != null)
//            {
//                yield return line;
//            }
//        }
//#endif

//        private static Dictionary<string, object> ParseRoutes(IEnumerable<string> config) // O(n * m)
//        {
//            var result = new Dictionary<string, object>();

//            var data = config
//                .Select(x => x.Split(' '))
//                .Select(x => (parts: x[0].Split('/'), endpoint: x[1])); // O(n * m)

//            foreach (var (parts, endpoint) in data) // O(n * m)
//            {
//                var current = result;

//                foreach (var part in parts) // O(m)
//                {
//                    if (!current.ContainsKey(part)) // O(1)
//                        current[part] = new Dictionary<string, object>(); // O(1)
//                    current = (Dictionary<string, object>)current[part]; // O(1)
//                }

//                current[EndpointKey] = endpoint; // O(1)
//            }

//            return result;
//        }

//        private static string Dispatch(Dictionary<string, object> routes, string path) // O(m)
//        {
//            var parts = path.Split('/'); // time: O(m) space: O(m)

//#if RECURSIVE
//			return DispatchRecursive(routes, parts, 0); // time: O(m) space: O(1), recursion: O(m)
//#else
//            return DispatchIterative(routes, parts); // time: O(m) space: O(m)
//#endif
//        }

//        private static string DispatchRecursive(Dictionary<string, object> routes, string[] parts, int index)
//        {
//            if (index == parts.Length)
//                return routes.TryGetValue(EndpointKey, out var endpoint) ? (string)endpoint : null; // O(1)

//            object value;

//            if (routes.TryGetValue(parts[index], out value)) // O(1)
//                if (value is Dictionary<string, object> d)
//                {
//                    var result = DispatchRecursive(d, parts, index + 1); // will recurse O(m) times
//                    if (result != null)
//                        return result;
//                }

//            if (routes.TryGetValue("X", out value)) // O(1)
//                if (value is Dictionary<string, object> d)
//                    return DispatchRecursive(d, parts, index + 1); // will recurse O(m) times

//            return null;
//        }

//        private static string DispatchIterative(Dictionary<string, object> routes, string[] parts)
//        {
//            var queue = new Stack<(Dictionary<string, object>, int)>();

//            queue.Push((routes, 0)); // O(1)

//            while (queue.Count > 0)
//            {
//                var (nested, index) = queue.Pop(); // O(1)

//                if (index == parts.Length) // exits after O(m) iterations
//                    if (nested.TryGetValue(EndpointKey, out var endpoint)) // O(1)
//                        return (string)endpoint;
//                    else
//                        continue;

//                object value;
//                if (nested.TryGetValue("X", out value)) // O(1)
//                    if (value is Dictionary<string, object> d)
//                        queue.Push((d, index + 1)); // O(1)

//                if (nested.TryGetValue(parts[index], out value)) // O(1)
//                    if (value is Dictionary<string, object> d)
//                        queue.Push((d, index + 1)); // O(1)
//            }

//            return null;
//        }
//    }

//    public static class WhyIsntThisInTheBcl//?
//    {
//        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
//        {
//            foreach (var item in source)
//                action(item);
//        }
//    }
//}