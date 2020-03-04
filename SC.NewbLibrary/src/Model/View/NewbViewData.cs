﻿using SC.NewbLibrary.Model.Data;
using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace SC.NewbLibrary.Model.View
{
    public class NewbViewData : NewbData
    {
        public NewbViewData()
        {
            if (EventHistory != null && EventHistory.Count() > 0)
            {
                var tmpTerm = EventHistory.OrderByDescending(a => a.ReviewStamp).FirstOrDefault(a => a.EventTerm != null && a.IsApproved == true).EventTerm;
                var tmpDefinition = EventHistory.OrderByDescending(a => a.ReviewStamp).FirstOrDefault(a => a.EventDefinition != null && a.IsApproved == true).EventDefinition;
                var tmpLink = EventHistory.OrderByDescending(a => a.ReviewStamp).FirstOrDefault(a => a.EventLink != null && a.IsApproved == true).EventLink;
                var tmpTerse = EventHistory.OrderByDescending(a => a.ReviewStamp).FirstOrDefault(a => a.EventTerse != null && a.IsApproved == true).EventTerse;

                if (tmpTerm != null && !tmpTerm.Equals(Term, StringComparison.OrdinalIgnoreCase))
                    Term = tmpTerm;
                if (tmpDefinition != null && !tmpDefinition.Equals(Definition, StringComparison.OrdinalIgnoreCase))
                    Definition = tmpDefinition;
                if (tmpLink != null && !tmpLink.Equals(Link, StringComparison.OrdinalIgnoreCase))
                    Link = tmpLink;
                if (tmpTerse != null && !tmpTerse.Equals(Term, StringComparison.OrdinalIgnoreCase))
                    Terse = tmpTerse;
            }
        }

        [JsonIgnore]
        public bool HasLink => !string.IsNullOrWhiteSpace(Link);
        [JsonIgnore]
        public int SuggestionCount => EventHistory.Count();
    }
}