namespace backend.Contracts;

public record SunriseSunsetDto(
    int Id,
    DateOnly Day,
    string Sunrise,
    string Sunset);
    