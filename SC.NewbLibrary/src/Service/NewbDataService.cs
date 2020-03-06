using Newtonsoft.Json;
using SC.NewbLibrary.Model.Data;
using SC.NewbLibrary.Model.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SC.NewbLibrary.Service
{
    public class NewbDataService
    {
        public Task<NewbViewData[]> GetNewbData()
        {
            // https://stackoverflow.com/questions/52279980/blazor-read-directory

            var rootPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(rootPath, "wwwroot/NEWB.json");
            var file = File.ReadAllText(filePath);
            var newbData = JsonConvert.DeserializeObject<NewbData[]>(file);

            //newbData.ToList().ForEach(data =>
            //    data.EventHistory.Add(new NewbDataEvent
            //    {
            //        EventDefinition = data.Definition,
            //        EventLink = data.Link,
            //        EventTerm = data.Term,
            //        EventTerse = data.Terse,
            //        //-- original creation --//
            //        EventStamp = new DateTime(2019, 07, 30, 12, 22, 0),
            //        EventUser = "A Dead Parrot",
            //        IsApproved = true
            //    }));
            //file = JsonConvert.SerializeObject(newbData);
            //File.WriteAllText(filePath, file);

            var result = new List<NewbViewData>();
            newbData.ToList().ForEach(a => result.Add(new NewbViewData
            {
                Definition = a.Definition,
                Link = a.Link,
                Term = a.Term,
                Terse = a.Terse,
                EventHistory = a.EventHistory
            }));
            result.ForEach(a => a.ReadEventSource());

            return Task.FromResult(result.ToArray());
        }
    }
}
