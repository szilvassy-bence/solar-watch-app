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
        _logger.LogInformation("Cities asked from the database.");
        return cities;
    }

    public async Task<City> GetCity(string city)
    {
        
        string cityJson = await _cityProvider.GetCity(city);
        return _jsonProcessor.ProcessCity(cityJson);
    }
}