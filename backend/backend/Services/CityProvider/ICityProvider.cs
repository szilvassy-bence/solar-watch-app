using backend.Models;

namespace backend.Services;

public interface ICityProvider
{
    Task<City> GetCity(string city);
}