using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sol3.Data.SQL.Weather.Models
{
    public class Sys : Entity
    {
        public Sys() { }
        public Sys(OpenWeatherMap.Models.Sys sys, DateTime timeStamp)
        {
            Type = sys.Type;
            Id = sys.Id;
            Country = sys.Country;
            Sunrise = sys.Sunrise;
            Sunset = sys.Sunset;
            Created = timeStamp;
        }

        public Guid WeatherResponseId { get; set; }
        public int Type { get; set; }
        public string Country { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }

        public virtual WeatherResponse WeatherResponse { get; set; }
    }
}
