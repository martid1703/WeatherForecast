using Microsoft.EntityFrameworkCore;
using WeatherForecastMvc.Models;

public class ForecastContext : DbContext
{
    public ForecastContext(DbContextOptions<ForecastContext> options)
        : base(options)
    {
        // Create the database if it doesn't exist
        Database.EnsureCreated();
    }

    public DbSet<DayForecast> DayForecast { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DayForecast>()
            .HasIndex(p => new { p.Date})
            .IsUnique(true);
    }
}
