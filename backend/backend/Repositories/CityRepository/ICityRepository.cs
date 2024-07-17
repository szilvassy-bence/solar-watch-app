using backend.Models;

namespace backend.Repositories.CityRepository;

public interface ICityRepository
{
    public Task<IEnumerable<City>> GetCities();
    public Task<City> GetCityByName(string city);
    public Task<City> GetCityById(int id);
    Task DeleteCity(City city);
}