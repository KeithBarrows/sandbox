//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WebRouting
//{    
//    class Program
//    {
        
//        private static List<string> routeAll(List<Route> routes, List<string> paths)
//        {
//            List<string> endpoints = new List<string>();
//            // Your code here

            
//            return endpoints;
//        }

//        /**
//         *    You probably won't need to edit anything below here.
//         */

//        public class Route
//        {
//            public string path { get; set; }
//            public string endpoint { get; set; }
//            public Route(String path, String endpoint)
//            {
//                this.path = path;
//                this.endpoint = endpoint;
//            }
//        }

//        static void Main(string[] args)
//        {
//            List<Route> routes = new List<Route>();

//            string line;
//            while ((line = Console.ReadLine()) != null && line.Length != 0)
//            {
//                string[] tokenizedLine = line.Split(' ');
//                routes.Add(new Route(tokenizedLine[0], tokenizedLine[1]));
//            }

//            List<string> paths = new List<string>();

//            while ((line = Console.ReadLine()) != null && line.Length != 0)
//            {
//                paths.Add(line);
//            }
			
//			var results = routeAll(routes, paths);
//            foreach (string endpoint in results)
//            {
//                Console.WriteLine(endpoint);
//            }

//            Console.ReadLine();
//        }
//    }
//}
