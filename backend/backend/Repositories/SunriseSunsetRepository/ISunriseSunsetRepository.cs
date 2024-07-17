using backend.Models;

namespace backend.Repositories.SunriseSunsetRepository;

public interface ISunriseSunsetRepository
{
    Task<IEnumerable<SunriseSunset>> GetAllSunriseSunsets();
    Task<SunriseSunset> GetSunriseSunset(string cityName, DateTime date);
    Task<SunriseSunset> GetSunriseSunsetById(int id);
    Task DeleteSunriseSunset(SunriseSunset sunriseSunset);
}