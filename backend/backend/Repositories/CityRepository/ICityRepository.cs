using backend.Models;

namespace backend.Repositories.CityRepository;

public interface ICityRepository
{
    public Task<City> GetCity(string city);
}