using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SC.NewbLibrary.Data
{
    public class NewbData
    {
        [StringLength(50)]
        [Key]
        public string Term { get; set; }
        public string Definition { get; set; }
        public string Terse { get; set; }
        public string Link { get; set; }

        [JsonIgnore]
        public string UriLink => HasLink ? Link : "[no link]";
        [JsonIgnore]
        public bool HasLink => !string.IsNullOrWhiteSpace(Link);
    }
}
