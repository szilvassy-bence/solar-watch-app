using backend.Models;

namespace backend.Services.JsonProcessor;

public interface IJsonProcessor
{
    public City ProcessCity(string json);
    public SunriseSunset ProcessSunriseSunset(string json, DateTime date);
}