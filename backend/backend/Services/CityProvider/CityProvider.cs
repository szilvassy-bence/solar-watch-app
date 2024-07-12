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
        _openWeatherApiKey = configuration["Solar:OpenWeatherMapKey"] ?? throw new InvalidOperationException();
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
                _logger.LogInformation("Calling OpenWeather API for Coordinates with url: {url}", url);
                throw new Exception("No city found for the given query.");
            }

            return cityJson;
        }
        else
        {
            _logger.LogInformation("Problem calling OpenWeather Api, status code: {statuscode}", response.StatusCode);
            throw new Exception($"Failed to retrieve data. Status code: {response.StatusCode}");
        }
    }
    
}