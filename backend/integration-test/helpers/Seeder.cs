using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace integration_test.helpers;

public class Seeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SolarContext _solarContext;

    public Seeder(
        RoleManager<IdentityRole> roleManager, 
        UserManager<IdentityUser> userManager,
        SolarContext solarContext)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _solarContext = solarContext;
    }

    public void InitializeSolarWatchForTests()
    {
        _solarContext.Cities.AddRange(GetSeedingCities());
        _solarContext.SunriseSunsets.AddRange(GetSeedingSunriseSunsets());
        _solarContext.SaveChanges();
    }

    public void ReinitializeSolarWatchDbForTests()
    {
        _solarContext.Cities.RemoveRange(_solarContext.Cities);
        _solarContext.SunriseSunsets.RemoveRange(_solarContext.SunriseSunsets);
        InitializeSolarWatchForTests();
    }

    public List<City> GetSeedingCities()
    {
        return new List<City>()
        {
            new City() { Id = 1, Country = "HU", Latitude = 100, Longitude = 100, Name = "Miskolc" },
            new City() { Id = 2, Country = "HU", Latitude = 123, Longitude = 123, Name = "Debrecen" },
            new City() { Id = 3, Country = "HU", Latitude = 200, Longitude = 200, Name = "Budapest" },
        };
    }

    public List<SunriseSunset> GetSeedingSunriseSunsets()
    {
        return new List<SunriseSunset>()
        {
            new SunriseSunset() { Id = 1, CityId = 1, Sunrise = new DateTime(2024, 01, 01), Sunset = new DateTime(2024, 01, 01) },
            new SunriseSunset() { Id = 2, CityId = 2, Sunrise = new DateTime(2024, 02, 01), Sunset = new DateTime(2024, 02, 01) },
            new SunriseSunset() { Id = 3, CityId = 3, Sunrise = new DateTime(2024, 03, 01), Sunset = new DateTime(2024, 03, 01) },
        };
    }

    public async Task ReinitializeIdentityUserDbForTests()
    {
        _solarContext.Users.RemoveRange(_solarContext.Users);
        _solarContext.Roles.RemoveRange(_solarContext.Roles);
        await InitializeIdentityUserDbForTests();
    }

    public async Task InitializeIdentityUserDbForTests()
    {
        var roles = AddRoles();
        var users = AddUsers();
        for (int i = 0; i < roles.Count; i++)
        {
            await _roleManager.CreateAsync(roles[i]);
            var identityResult = await _userManager.CreateAsync(users[i], "password");
            if (identityResult.Succeeded) await _userManager.AddToRoleAsync(users[i], roles[i].Name);
        }

        var city = await _solarContext.Cities.FirstOrDefaultAsync(c => c.Id == 1);
        if (city is null) throw new Exception();
        
        users[1].Favorites.Add(city);
    }

    public List<IdentityRole> AddRoles()
    {
        return new List<IdentityRole>()
        {
            new IdentityRole("Admin"),
            new IdentityRole("User"),
        };
    }

    public List<AppUser> AddUsers()
    {
        return new List<AppUser>()
        {
            new AppUser() { UserName = "admin", Email = "admin@admin" },
            new AppUser() { UserName = "user", Email = "user@user" },
        };
    }
}