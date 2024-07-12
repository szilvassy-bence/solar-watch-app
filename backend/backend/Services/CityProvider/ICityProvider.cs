using backend.Models;

namespace backend.Services;

public interface ICityProvider
{
    Task<string> GetCity(string city);
}