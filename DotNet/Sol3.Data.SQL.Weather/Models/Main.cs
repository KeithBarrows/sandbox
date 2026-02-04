using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sol3.Data.SQL.Weather.Models
{
    public class Main : Entity
    {
        public Main() { }
        public Main(OpenWeatherMap.Models.Main main, DateTime timeStamp)   // , Guid weatherResponseId) : base(weatherResponseId)
        {
            Temp = main.Temp;
            FeelsLike = main.FeelsLike;
            TempMin = main.TempMin;
            TempMax = main.TempMax;
            Pressure = main.Pressure;
            Humidity = main.Humidity;
            SeaLevel = main.SeaLevel;
            GrndLevel = main.GrndLevel;
            Created = timeStamp;
        }

        public Guid WeatherResponseId { get; set; }
        public double Temp { get; set; }
        public double FeelsLike { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public int SeaLevel { get; set; }
        public int GrndLevel { get; set; }

        public virtual WeatherResponse WeatherResponse { get; set; }
    }
}
