using System;
using System.Collections.Generic;

namespace Sol3.House.ApiModel
{
    public class HouseVote
    {
        public bool IsActive { get; set; }
        public DateTime Added { get; set; }
        public DateTime FirstOnTheMarket { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public int Zip5 { get; set; }
        public int Zip4 { get; set; }

        public int Score
        {
            get
            {
                var score = 0;
                Details.ForEach(detail => score += detail.VoteValue * detail.Weight);
                return score;
            }
            private set { }
        }

        public List<VoteDetail> Details { get; set; }
    }
}
