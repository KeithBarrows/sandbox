using SC.NewbLibrary.Model.Data;
using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace SC.NewbLibrary.Model.View
{
    public class NewbViewData : NewbData
    {
        public void ReadEventSource()
        {
            if (EventHistory != null && EventHistory.Count() > 0)
            {
                var tmpTerm = EventHistory.OrderByDescending(a => a.ReviewStamp).FirstOrDefault(a => a.EventTerm != null && a.IsApproved == true).EventTerm;
                var tmpDefinition = EventHistory.OrderByDescending(a => a.ReviewStamp).FirstOrDefault(a => a.EventDefinition != null && a.IsApproved == true).EventDefinition;
                var tmpLink = EventHistory.OrderByDescending(a => a.ReviewStamp).FirstOrDefault(a => a.EventLink != null && a.IsApproved == true).EventLink;
                var tmpTerse = EventHistory.OrderByDescending(a => a.ReviewStamp).FirstOrDefault(a => a.EventTerse != null && a.IsApproved == true).EventTerse;

                if (!tmpTerm.IsEqualTo(Term))
                    Term = tmpTerm;
                if (!tmpDefinition.IsEqualTo(Definition))
                    Definition = tmpDefinition;
                if (!tmpLink.IsEqualTo(Link))
                    Link = tmpLink;
                if (!tmpTerse.IsEqualTo(Terse))
                    Terse = tmpTerse;
            }
        }

        [JsonIgnore]
        public bool HasLink => !string.IsNullOrWhiteSpace(Link);
        [JsonIgnore]
        public int SuggestionCount => EventHistory.Count();
    }
}
