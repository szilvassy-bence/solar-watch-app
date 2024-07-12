using backend.Models;
using backend.Services;
using backend.Services.JsonProcessor;

namespace backend.Repositories.CityRepository;

public class CityRepository : ICityRepository
{
    private readonly ICityProvider _cityProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public CityRepository(ICityProvider cityProvider, IJsonProcessor jsonProcessor)
    {
        _cityProvider = cityProvider;
        _jsonProcessor = jsonProcessor;
    }

    public async Task<City> GetCity(string city)
    {
        string cityJson = await _cityProvider.GetCity(city);
        return _jsonProcessor.ProcessCity(cityJson);
    }
}