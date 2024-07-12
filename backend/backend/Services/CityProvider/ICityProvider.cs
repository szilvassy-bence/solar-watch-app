using backend.Models;

namespace backend.Services.CityProvider;

public interface ICityProvider
{
    Task<string> GetCity(string city);
}