using backend.Models;

namespace backend.Repositories.SunriseSunsetRepository;

public interface ISunriseSunsetRepository
{
    public Task<SunriseSunset> GetSunriseSunset(string cityName, DateTime date);
}