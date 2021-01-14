using ElectionTracker.NyTimes;
using Newtonsoft.Json;
using System;
using System.IO;

namespace ElectionTracker
{
    class Program
    {
        private static string nyTimesEndpoint = "https://static01.nyt.com/elections-assets/2020/data/api/2020-11-03/race-page/pennsylvania/president.json";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new ModifiedDataContractResolver(),
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Formatting = Formatting.Indented,
                CheckAdditionalContent = true,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                MaxDepth = 15,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.All,
            };

            var fileContents = File.ReadAllText(@"C:\Users\keith\Documents\Election Fraud\https__static01_nyt.com--elections-assets--2020--data--api--2020-11-03--race-page--pennsylvania--president.json");
            var root = JsonConvert.DeserializeObject<Root>(fileContents, settings);

            var timeSeries = root.Data.Races[0].TimeSeries;


        }
    }
}
