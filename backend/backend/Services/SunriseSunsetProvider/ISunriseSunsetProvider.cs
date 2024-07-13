namespace backend.Services.SunriseSunsetProvider;

public interface ISunriseSunsetProvider
{
    Task<string> GetSunriseSunset(double lat, double lon, DateTime date);
}