using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sol3.House.ApiModel;
using Sol3.ListAlgorithms.Model;
using System;
using System.Collections.Generic;

namespace Sol3.House.Features.Base
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        private readonly ILogger _logger;
        private static readonly Random _random = new Random();

        public BaseController(ILogger logger)
        {
            _logger = logger;
        }

        public ILogger Logger => _logger;

        internal List<VoteDetail> AddMoqDetails()
        {
            var result = new List<VoteDetail>
            {
                        new VoteDetail { Title = "Does it feel spacious?", Weight = 10, VoteValue = GetRandom() },
                        new VoteDetail { Title = "Open concept?", Weight = 9, VoteValue = GetRandom() },
                        new VoteDetail { Title = "3 Bedrooms?", Weight = 8, VoteValue = GetRandom() },
                        new VoteDetail { Title = ">1 Acre?", Weight = 7, VoteValue = GetRandom() },
                        new VoteDetail { Title = "Office?", Weight = 6, VoteValue = GetRandom() },
                        new VoteDetail { Title = "Sewing Room?", Weight = 5, VoteValue = GetRandom() },
                        new VoteDetail { Title = "Swimming Pool?", Weight = 4, VoteValue = GetRandom() },
                        new VoteDetail { Title = "Workshop?", Weight = 3, VoteValue = GetRandom() },
                        new VoteDetail { Title = ">2 Acre?", Weight = 2, VoteValue = GetRandom() },
                        new VoteDetail { Title = ">3 Acre?", Weight = 1, VoteValue = GetRandom() },
            };
            var rank = new Rank<VoteDetail>();
            rank.CreateTree(result);
            return result;
        }

        private int GetRandom()
        {
            lock (_random) // synchronize
            {
                return _random.Next(0, 10);
            }
        }
    }
}
