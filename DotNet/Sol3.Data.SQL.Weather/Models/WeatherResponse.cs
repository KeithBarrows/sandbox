using System;
using System.Collections.Generic;

namespace Sol3.Data.SQL.Weather.Models
{
    public partial class WeatherResponse : Entity
    {
        public WeatherResponse()
        {
            Weathers = new HashSet<Weather>();
        }
        public WeatherResponse(OpenWeatherMap.Models.WeatherResponse weatherResponse, DateTime timeStamp)
        {
            WeatherResponseId = Guid.NewGuid();

            Weathers = new HashSet<Weather>();

            Base = weatherResponse.Base;
            Visibility = weatherResponse.Visibility;
            Dt = weatherResponse.Dt;
            Timezone = weatherResponse.Timezone;
            Id = weatherResponse.Id;
            Name = weatherResponse.Name;
            Created = timeStamp;

            Coord = new Coord(weatherResponse.Coord, timeStamp);
            Main = new Main(weatherResponse.Main, timeStamp);
            Wind = new Wind(weatherResponse.Wind, timeStamp);
            Cloud = new Cloud(weatherResponse.Clouds);
            Sys = new Sys(weatherResponse.Sys, timeStamp);

            if (weatherResponse.Weather != null)
                weatherResponse.Weather.ForEach(weather => Weathers.Add(new Models.Weather(weather)));
        }


        public Guid WeatherResponseId { get; set; }
        public string Base { get; set; }
        public int? Visibility { get; set; }
        public int? Dt { get; set; }
        public int? Timezone { get; set; }
        public string Name { get; set; }

        public virtual Cloud Cloud { get; set; }
        public virtual Coord Coord { get; set; }
        public virtual Main Main { get; set; }
        public virtual Wind Wind { get; set; }
        public virtual Sys Sys { get; set; }
        public virtual ICollection<Weather> Weathers { get; set; }
    }
}
