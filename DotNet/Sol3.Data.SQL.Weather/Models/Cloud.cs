using System;

namespace Sol3.Data.SQL.Weather.Models
{
    public partial class Cloud
    {
        public Cloud() { }
        public Cloud(OpenWeatherMap.Models.Clouds clouds)
        {
            All = clouds.All;
        }

        public Guid WeatherResponseId { get; set; }
        public int? All { get; set; }

        public virtual WeatherResponse WeatherResponse { get; set; }
    }
}
