using backend.Models;

namespace backend.Repositories.CityRepository;

public interface ICityRepository
{
    public Task<IEnumerable<City>> GetCities();
    public Task<City> GetCity(string city);
}