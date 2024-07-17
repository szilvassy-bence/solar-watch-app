using backend.Controllers;
using backend.Data;
using backend.Models;
using backend.Repositories.CityRepository;
using backend.Services.JsonProcessor;
using backend.Services.SunriseSunsetProvider;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.SunriseSunsetRepository;

public class SunriseSunsetRepository : ISunriseSunsetRepository
{
    private readonly ICityRepository _cityRepository;
    private readonly ILogger<SunriseSunsetController> _logger;
    private readonly SolarContext _context;
    private readonly ISunriseSunsetProvider _sunriseSunsetProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SunriseSunsetRepository(
        ICityRepository cityRepository, 
        ILogger<SunriseSunsetController> logger, 
        SolarContext context,
        ISunriseSunsetProvider sunriseSunsetProvider,
        IJsonProcessor jsonProcessor)
    {
        _cityRepository = cityRepository;
        _logger = logger;
        _context = context;
        _sunriseSunsetProvider = sunriseSunsetProvider;
        _jsonProcessor = jsonProcessor;
    }
    
    public async Task<IEnumerable<SunriseSunset>> GetAllSunriseSunsets()
    {
        return await _context.SunriseSunsets.ToListAsync();
    }

    public async Task<SunriseSunset> GetSunriseSunset(string cityName, DateTime date)
    {
        var city = await _cityRepository.GetCityByName(cityName);
        if (date == new DateTime()) date = DateTime.Now;

        // check if city already has sun information for the required date and return
        if (city.SunriseSunsets.Count > 0)
        {
            var sunInfoOnDate = city.SunriseSunsets.FirstOrDefault(x => x.Day == DateOnly.FromDateTime(date));
            if (sunInfoOnDate != null) return sunInfoOnDate;
        }

        // fetch the new sun information for the new date
        var json = await _sunriseSunsetProvider.GetSunriseSunset(city.Latitude, city.Longitude, date);
        var suninfo = _jsonProcessor.ProcessSunriseSunset(json, date);
        suninfo.CityId = city.Id;
        var sunInfoEntry = _context.SunriseSunsets.Add(suninfo);
        await _context.SaveChangesAsync();
        return sunInfoEntry.Entity;
    }

    public async Task<SunriseSunset> GetSunriseSunsetById(int id)
    {
        SunriseSunset? sunriseSunset = await _context.SunriseSunsets.FindAsync(id);
        if (sunriseSunset is null) throw new ArgumentException("No sunrise sunset found for the given id.");
        return sunriseSunset;
    }
    
    public async Task DeleteSunriseSunset(SunriseSunset sunriseSunset)
    {
        _context.SunriseSunsets.Remove(sunriseSunset);
        await _context.SaveChangesAsync();
    }
}