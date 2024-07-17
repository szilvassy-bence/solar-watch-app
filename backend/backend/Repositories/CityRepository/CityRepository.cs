using backend.Data;
using backend.Models;
using backend.Services;
using backend.Services.CityProvider;
using backend.Services.JsonProcessor;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.CityRepository;

public class CityRepository : ICityRepository
{
    private readonly ILogger<CityRepository> _logger;
    private readonly ICityProvider _cityProvider;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly SolarContext _context;

    public CityRepository(
        ILogger<CityRepository> logger,
        ICityProvider cityProvider, 
        IJsonProcessor jsonProcessor, 
        SolarContext context
        )
    {
        _logger = logger;
        _cityProvider = cityProvider;
        _jsonProcessor = jsonProcessor;
        _context = context;
    }

    public async Task<IEnumerable<City>> GetCities()
    {
        var cities = await _context.Cities
            .ToListAsync();
        return cities;
    }

    public async Task<City> GetCityByName(string city)
    {
        var dbCity = await _context.Cities
            .Include(c => c.SunriseSunsets)
            .FirstOrDefaultAsync(c => c.Name == city);
        if (dbCity is null)
        {
            string cityJson = await _cityProvider.GetCity(city);
            var c = _jsonProcessor.ProcessCity(cityJson);
            var cityEntry = _context.Cities.Add(c);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New city entry is created.");
            return cityEntry.Entity;
        }
        _logger.LogInformation("City was found in database.");
        return dbCity;
    }

    public async Task<City> GetCityById(int id)
    {
        City? city = await _context.Cities
            .Include(x => x.SunriseSunsets)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (city is null)
        {
            throw new ArgumentException("No city found for the given id.");
        }

        return city;
    }
    
    public async Task DeleteCity(City city)
    {
        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();
    }
}