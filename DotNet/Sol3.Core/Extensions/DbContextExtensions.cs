using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sol3.Data.SQL.Weather;
using System.Configuration;

namespace Sol3.Core
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddWeatherDbContext(this IServiceCollection services)
            => services.AddDbContext<WeatherContext>(options => options.UseSqlServer(ConfigurationManager.ConnectionStrings["WeatherConnection"].ConnectionString));
    }
}
