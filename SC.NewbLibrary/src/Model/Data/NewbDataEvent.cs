using System;
using System.ComponentModel.DataAnnotations;

namespace SC.NewbLibrary.Model.Data
{
    public class NewbDataEvent
    {
        [StringLength(50)]
        public string EventTerm { get; set; }
        public string EventDefinition { get; set; }
        public string EventTerse { get; set; }
        public string EventLink { get; set; }
        public DateTime EventStamp { get; set; } = DateTime.UtcNow;
        public string EventUser { get; set; }
        public DateTime? ReviewStamp { get; set; }
        public string ReviewUser { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsRejected { get; set; } = false;
    }
}
