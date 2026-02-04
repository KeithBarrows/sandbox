using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sol3.House.ApiModel;
using Sol3.House.Features.Base;

namespace Sol3.House.Features.Vote
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : BaseController
    {
        public VoteController(ILogger<VoteController> logger) : base(logger) { }

        [HttpGet]
        public HouseVote Get()
        {
            return new HouseVote();
        }
    }
}
