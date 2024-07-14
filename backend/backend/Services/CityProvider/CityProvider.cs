using System.Text.Json;
using backend.Models;

namespace backend.Services.CityProvider;

public class CityProvider : ICityProvider
{
    private readonly ILogger<CityProvider> _logger;
    private readonly string _openWeatherApiKey;

    public CityProvider(ILogger<CityProvider> logger, IConfiguration configuration)
    {
        _logger = logger;
        _openWeatherApiKey = Environment.GetEnvironmentVariable("OPENWEATHERAPIKEY") 
                             ?? throw new ArgumentException("Invalid OpenWeatherAPI key.");
    }

    public async Task<string> GetCity(string city)
    {
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={_openWeatherApiKey}";

        using var client = new HttpClient();
        
        _logger.LogInformation("Calling OpenWeather API for Coordinates with url: {url}", url);
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var cityJson = await response.Content.ReadAsStringAsync();

            if (cityJson.Trim() == "[]")
            {
                throw new ArgumentException("No city found for the given query.");
            }

            return cityJson;
        }
        else
        {
            throw new Exception($"Failed to retrieve data. Status code: {response.StatusCode}");
        }
    }
    
}