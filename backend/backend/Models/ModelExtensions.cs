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

    public static UserDto AsDto(this AppUser user)
    {
        return new UserDto(
            user.Id,
            user.UserName,
            user.Email,
            user.Favorites);
    }
    
    public static CityDto AsDto(this City city)
    {
        return new CityDto(
            city.Id,
            city.Name,
            city.Country
            );
    }
}