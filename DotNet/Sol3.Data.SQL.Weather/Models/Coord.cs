using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sol3.Data.SQL.Weather.Models
{
    public class Coord : Entity
    {
        public Coord() { }
        public Coord(OpenWeatherMap.Models.Coord coord, DateTime timeStamp)   // , Guid weatherResponseId) : base(weatherResponseId)
        {
            Lon = coord.Lon;
            Lat = coord.Lat;
            Created = timeStamp;
        }

        public Guid WeatherResponseId { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }

        public virtual WeatherResponse WeatherResponse { get; set; }
    }
}
