using Microsoft.EntityFrameworkCore;
using Sol3.Data.SQL.Weather.Models;

namespace Sol3.Data.SQL.Weather
{
    public class WeatherContext : DbContext
    {
        //public WeatherContext() { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=Peregrine;Database=Weather;Trusted_Connection=True;");
        //}

        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
        {
        }

        public DbSet<Cloud> Clouds { get; set; }
        public DbSet<Coord> Coords { get; set; }
        public DbSet<Main> Mains { get; set; }
        public DbSet<Sys> Sys { get; set; }
        public DbSet<Models.Weather> Weather { get; set; }
        public DbSet<WeatherResponse> WeatherResponses { get; set; }
        public DbSet<Wind> Winds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Cloud>(entity =>
            {
                entity.HasKey(e => e.WeatherResponseId);

                entity.ToTable("Cloud");

                entity.Property(e => e.WeatherResponseId).ValueGeneratedNever();

                entity.HasOne(d => d.WeatherResponse)
                    .WithOne(p => p.Cloud)
                    .HasForeignKey<Cloud>(d => d.WeatherResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cloud_WeatherResponse");
            });

            modelBuilder.Entity<Coord>(entity =>
            {
                entity.HasKey(e => e.WeatherResponseId);

                entity.ToTable("Coords");

                entity.Property(e => e.WeatherResponseId).ValueGeneratedNever();

                entity.HasOne(d => d.WeatherResponse)
                    .WithOne(p => p.Coord)
                    .HasForeignKey<Coord>(d => d.WeatherResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Coord_WeatherResponse");
            });

            modelBuilder.Entity<Main>(entity =>
            {
                entity.HasKey(e => e.WeatherResponseId);

                entity.ToTable("Mains");

                entity.Property(e => e.WeatherResponseId).ValueGeneratedNever();

                entity.HasOne(d => d.WeatherResponse)
                    .WithOne(p => p.Main)
                    .HasForeignKey<Main>(d => d.WeatherResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Main_WeatherResponse");
            });

            modelBuilder.Entity<Sys>(entity =>
            {
                entity.HasKey(e => e.WeatherResponseId);

                entity.ToTable("Sys");

                entity.Property(e => e.WeatherResponseId).ValueGeneratedNever();

                entity.HasOne(d => d.WeatherResponse)
                    .WithOne(p => p.Sys)
                    .HasForeignKey<Sys>(d => d.WeatherResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sys_WeatherResponse");
            });

            modelBuilder.Entity<Wind>(entity =>
            {
                entity.HasKey(e => e.WeatherResponseId);

                entity.ToTable("Winds");

                entity.Property(e => e.WeatherResponseId).ValueGeneratedNever();

                entity.HasOne(d => d.WeatherResponse)
                    .WithOne(p => p.Wind)
                    .HasForeignKey<Wind>(d => d.WeatherResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wind_WeatherResponse");
            });



            modelBuilder.Entity<Models.Weather>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.WeatherResponseId });

                entity.ToTable("Weather");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Main)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.WeatherResponse)
                    .WithMany(p => p.Weathers)
                    .HasForeignKey(d => d.WeatherResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weather_WeatherResponse");
            });

            modelBuilder.Entity<WeatherResponse>(entity =>
            {
                entity.HasKey(e => e.WeatherResponseId);

                entity.ToTable("WeatherResponse");

                entity.Property(e => e.WeatherResponseId).ValueGeneratedNever();

                entity.Property(e => e.Base)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
