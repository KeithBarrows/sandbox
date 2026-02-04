using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sol3.House.ApiModel;
using Sol3.House.Features.Base;
using Sol3.ListAlgorithms.Model;
using System.Collections.Generic;
using static Sol3.ListAlgorithms.Model.PairwiseModel;

namespace Sol3.House.Features.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : BaseController
    {
        public SetupController(ILogger<SetupController> logger) : base(logger) { }

        [HttpGet]
        [Route("Pairs")]
        public List<PairModel> GetPairList()
        {
            var model = new PairwiseModel();
            model.AddItem("Open concept");
            model.AddItem(">1 Acre");
            model.AddItem(">2 Acres");
            model.AddItem(">3 Acres");
            model.AddItem(">5 Acres");
            model.AddItem(">9 Acres");
            model.AddItem("House feels big and open");
            model.AddItem("Swimming pool");
            model.AddItem("Office");
            model.AddItem("Sewing Room");
            model.AddItem(">600 sqft Workshop");
            model.AddItem("2+ Bathrooms");
            model.AddItem("3+ Bathrooms");
            model.AddItem("Chef's Kitchen");
            model.AddItem(">1800 sqft");
            model.AddItem("High ceilings");
            model.AddItem("Walk in pantry");
            model.AddItem("Extra closet space");
            model.AddItem("Country sink");
            model.AddItem("Vegetable sink");
            model.AddItem("Double ovens");
            model.AddItem("Single floor");
            model.PrepForSort();
            var result = model.GetListToCompare();

            return result;
        }

        [HttpGet]
        [Route("Votes")]
        public List<HouseVote> Get()
        {
            var result = new List<HouseVote>
            {
                new HouseVote { StreetAddress1 = "11415 NE Highway 315", City = "Fort Mc Coy", Zip5 = 32134, Details = AddMoqDetails(), Url = "https://www.zillow.com/homedetails/11415-NE-Highway-315-Fort-Mc-Coy-FL-32134/87662888_zpid/", FirstOnTheMarket = new System.DateTime(2021,11,18), IsActive = true },
                new HouseVote { StreetAddress1 = "15750 SE Highway 42", City = "Weirsdale", Zip5 = 32195, Details = AddMoqDetails(), Url = "https://www.zillow.com/homedetails/15750-SE-Highway-42-Weirsdale-FL-32195/130801851_zpid/" , FirstOnTheMarket = new System.DateTime(2021,12,5), IsActive = true },
            };

            return result;
        }
    }
}
