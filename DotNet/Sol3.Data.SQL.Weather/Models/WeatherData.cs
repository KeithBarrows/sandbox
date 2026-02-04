using System;

namespace Sol3.Data.SQL.Weather.Models
{
    public partial class Weather : Entity
    {
        public Weather() { }
        public Weather(OpenWeatherMap.Models.Weather weather)
        {
            Id = weather.Id;
            Main = weather.Main;
            Description = weather.Description;
            Icon = weather.Icon;
        }

        public Guid WeatherResponseId { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public virtual WeatherResponse WeatherResponse { get; set; }
    }
}
