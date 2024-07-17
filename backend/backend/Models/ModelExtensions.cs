using backend.Contracts;

namespace backend.Models;

public static class ModelExtensions
{
    public static SunriseSunsetDto AsDto(this SunriseSunset sunriseSunset)
    {
        return new SunriseSunsetDto(
            sunriseSunset.Id,
            sunriseSunset.Day,
            sunriseSunset.Sunrise.ToString("HH:mm:ss"),
            sunriseSunset.Sunset.ToString("HH:mm:ss"));
    }
}