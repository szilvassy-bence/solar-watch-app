using System.Text.Json;
using backend.Models;

namespace backend.Services;

public class CityProvider : ICityProvider
{

    private readonly ILogger<CityProvider> _logger;
    private readonly string _openWeatherApiKey;

    public CityProvider(ILogger<CityProvider> logger, string openWeatherApiKey)
    {
        _logger = logger;
        _openWeatherApiKey = openWeatherApiKey;
    }

    public async Task<City> GetCity(string city)
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
    
    private City Process(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            throw new ArgumentException("Input json data cannot be empty.");
        }
 
        JsonDocument jd = JsonDocument.Parse(json);
        JsonElement root = jd.RootElement[0];
        var lat = GetDoubleProperty(root, "lat");
        var lon = root.GetProperty("lon");
        var name = root.GetProperty("name");
        var country = root.GetProperty("country");
        
        City city;
        JsonElement state;
        if (root.TryGetProperty("state", out state))
            city = new City
            {
                Latitude = lat.GetDouble(), 
                Longitude = lon.GetDouble(), 
                Name = name.GetString(),
                Country = country.GetString(), 
                State = state.GetString()
            };
        else
            city = new City
            {
                Latitude = lat.GetDouble(), 
                Longitude = lon.GetDouble(), 
                Name = name.GetString(),
                Country = country.GetString()
            };

        return city;
    }
}