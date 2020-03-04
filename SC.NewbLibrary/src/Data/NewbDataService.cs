using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace SC.NewbLibrary.Data
{
    public class NewbDataService
    {
        public Task<NewbData[]> GetNewbData()
        {
            // https://stackoverflow.com/questions/52279980/blazor-read-directory

            var rootPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(rootPath, "Data\\NEWB.json");
            var file = File.ReadAllText(filePath);
            var obj = JsonConvert.DeserializeObject<NewbData[]>(file);
            return Task.FromResult(obj);
        }
    }
}
