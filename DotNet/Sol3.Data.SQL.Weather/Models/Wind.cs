using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sol3.Data.SQL.Weather.Models
{
    public class Wind : Entity
    {
        public Wind() { }
        public Wind(OpenWeatherMap.Models.Wind wind, DateTime timeStamp)   // , Guid weatherResponseId) : base(weatherResponseId)
        {
            Speed = wind.Speed;
            Deg = wind.Deg;
            Gust = wind.Gust;
            Created = timeStamp;
        }

        public Guid WeatherResponseId { get; set; }
        public double Speed { get; set; }
        public int Deg { get; set; }
        public double Gust { get; set; }

        public virtual WeatherResponse WeatherResponse { get; set; }
    }
}
