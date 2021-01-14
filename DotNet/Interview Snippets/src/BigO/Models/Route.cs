using System.Collections.Generic;
using BigO.Extensions;

namespace BigO.Models
{
    public class Route
    {
        public string Path { get; set; }
        public string Endpoint { get; set; }
        public string[] PathParts => Path.Split('/');
    }

    public class Route2
    {
        public Route2()
        {
            Children = new Dictionary<string, Route2>();
        }

        public string Parent { get; set; }
        public string Path { get; set; }
        public Dictionary<string, Route2> Children { get; set; }
        public string Endpoint { get; set; }
        public int WildcardCount { get; set; }
        public bool IsValidEndpoint => !Endpoint.IsEmpty();
        public bool IsWildcard => Path.ToUpper() == "X";
    }
}