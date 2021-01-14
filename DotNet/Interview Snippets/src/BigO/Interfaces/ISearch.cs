using System.Collections.Generic;

namespace BigO.Interfaces
{
    public interface ISearch
    {
        string Execute(string path);
        IEnumerable<string> Execute(IEnumerable<string> paths);
    }
}
