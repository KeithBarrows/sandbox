using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WeatherStation.Data;

// Lets `dotnet ef migrations add` run standalone against this class library without
// booting the Blazor host. Points at the standard local Supabase CLI defaults.
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WeatherDbContext>
{
    public WeatherDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WeatherDbContext>();
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=54322;Database=postgres;Username=postgres;Password=postgres");
        return new WeatherDbContext(optionsBuilder.Options);
    }
}
