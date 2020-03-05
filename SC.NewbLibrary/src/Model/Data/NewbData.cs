using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SC.NewbLibrary.Model.Data
{
    public class NewbData
    {
        [StringLength(50)]
        [Key]
        public string Term { get; set; }
        public string Definition { get; set; }
        public string Terse { get; set; }
        public string Link { get; set; }

        public List<NewbDataEvent> EventHistory { get; set; } = new List<NewbDataEvent>();
    }
}
