using SC.NewbLibrary.Model.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace SC.NewbLibrary.Model.View
{
    public class NewbViewData //: NewbData
    {
        [StringLength(50)]
        [Key]
        public string Term { get; set; }
        public string Definition
        {
            get
            {
                var original = EventHistory.OrderBy(a => a.ReviewStamp).FirstOrDefault(a => a.IsApproved == true)?.EventDefinition ?? "";
                var latest = EventHistory.OrderByDescending(a => a.ReviewStamp).FirstOrDefault(a => a.EventDefinition != null && a.IsApproved == true)?.EventDefinition ?? "";
                if (original.IsEqualTo(latest))
                    return original;
                else
                    return latest;
            }
            private set { }
        }
        public string Terse
        {
            get
            {
                var original = EventHistory.OrderBy(a => a.ReviewStamp).FirstOrDefault(a => a.IsApproved == true)?.EventTerse ?? "";
                var latest = EventHistory.OrderByDescending(a => a.ReviewStamp).FirstOrDefault(a => a.EventTerse != null && a.IsApproved == true)?.EventTerse ?? "";
                if (original.IsEqualTo(latest))
                    return original;
                else
                    return latest;
            }
            private set { }
        }
        public string Link
        {
            get
            {
                var original = EventHistory.OrderBy(a => a.ReviewStamp).FirstOrDefault(a => a.IsApproved == true)?.EventLink ?? "";
                var latest = EventHistory.OrderByDescending(a => a.ReviewStamp).FirstOrDefault(a => a.EventLink != null && a.IsApproved == true)?.EventLink ?? "";
                if (original.IsEqualTo(latest))
                    return original;
                else
                    return latest;
            }
            private set { }
        }
        public List<NewbDataEvent> EventHistory { get; set; } = new List<NewbDataEvent>();


        [JsonIgnore]
        public bool HasLink => !string.IsNullOrWhiteSpace(Link);
        [JsonIgnore]
        public int SuggestionCount => EventHistory.Count();

        public NewbData DataModel
        {
            get
            {
                return new NewbData
                {
                    Term = this.Term,
                    EventHistory = this.EventHistory,
                };
            }
        }

        private void EventHistoryRunner()
        {
            if (EventHistory.Count <= 0) { }
            if (EventHistory.Count == 1 || (EventHistory.Count >= 1 && EventHistory.All(a => !a.IsApproved && !a.IsRejected))) { }
            else { }
        }
    }
}
