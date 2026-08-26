using Microsoft.EntityFrameworkCore;
using WeatherStation.Data.Entities;

namespace WeatherStation.Data;

public class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
{
    public DbSet<WeatherObservation> WeatherObservations => Set<WeatherObservation>();
    public DbSet<ForecastObservation> ForecastObservations => Set<ForecastObservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherObservation>(e =>
        {
            // Dedupe guarantee for the collector, and the range-scan index for chart queries.
            // The default backfills every pre-multi-location row to "citra" so adding this
            // NOT NULL column to an already-populated table doesn't break.
            e.Property(o => o.LocationKey).HasMaxLength(32).HasDefaultValue("citra").IsRequired();
            e.HasIndex(o => new { o.LocationKey, o.ObservedAtUtc }).IsUnique();

            e.Property(o => o.Source).HasMaxLength(16);

            e.Property(o => o.PressureInHg).HasPrecision(6, 2);
            e.Property(o => o.PrecipInPerHr).HasPrecision(6, 2);

            e.Property(o => o.ObservedAtUtc).HasColumnType("timestamptz");
            e.Property(o => o.CreatedAtUtc).HasColumnType("timestamptz");
        });

        modelBuilder.Entity<ForecastObservation>(e =>
        {
            // Dedupe guarantee: one row per (location, issued-at, forecast-hour) triple.
            e.Property(f => f.LocationKey).HasMaxLength(32).HasDefaultValue("citra").IsRequired();
            e.HasIndex(f => new { f.LocationKey, f.IssuedAtUtc, f.ForecastForUtc }).IsUnique();

            e.Property(f => f.PressureInHg).HasPrecision(6, 2);
            e.Property(f => f.PrecipInPerHr).HasPrecision(6, 2);

            e.Property(f => f.IssuedAtUtc).HasColumnType("timestamptz");
            e.Property(f => f.ForecastForUtc).HasColumnType("timestamptz");
            e.Property(f => f.CreatedAtUtc).HasColumnType("timestamptz");
        });
    }
}
