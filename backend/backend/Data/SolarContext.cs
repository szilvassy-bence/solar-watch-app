using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class SolarContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public SolarContext(DbContextOptions<SolarContext> options) : base(options)
    {}
    
    public DbSet<City> Cities { get; set; }
    public DbSet<SunriseSunset> SunriseSunsets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<City>()
            .HasMany(c => c.SunriseSunsets)
            .WithOne(ss => ss.City)
            .HasForeignKey(ss => ss.CityId);

        modelBuilder.Entity<City>()
            .HasOne(c => c.User)
            .WithMany(u => u.Favorites);
        
        modelBuilder.Entity<City>()
            .HasData(
                new City
                {
                    Id = 1, Name = "London", Latitude = 51.509865, Longitude = -0.118092, Country = "GB",
                    State = "England"
                },
                new City
                {
                    Id = 2, Name = "Budapest", Latitude = 47.497913, Longitude = 19.040236, Country = "HU"
                },
                new City
                {
                    Id = 3, Name = "Paris", Latitude = 48.864716, Longitude = 2.349014, Country = "FR",
                    State = "Ile-de-France"
                }
            );
    }
}