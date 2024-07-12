using System.Text.Json;
using backend.Models;

namespace backend.Services.JsonProcessor;

public class JsonProcessor : IJsonProcessor
{
    public City ProcessCity(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            throw new JsonException("Cannot process empty JSON.");
        }
        
        JsonDocument jsonDocument = JsonDocument.Parse(json);

        JsonElement jsonElement = jsonDocument.RootElement[0];

        string name = GetStringProperty(jsonElement, "name");
        string country = GetStringProperty(jsonElement, "country");
        double lat = GetDoubleProperty(jsonElement, "lat");
        double lon = GetDoubleProperty(jsonElement, "lon");

        string? state = jsonElement.TryGetProperty("state", out JsonElement stateElement)
            ? stateElement.GetString()
            : null;

        var city = new City
        {
            Name = name,
            Country = country,
            Latitude = lat,
            Longitude = lon,
            State = state
        };

        return city;
    }
    
    public SunriseSunset ProcessSunriseSunset(string json)
    {
        throw new NotImplementedException();
    }

    private string GetStringProperty(JsonElement jsonElement, string property)
    {
        if (jsonElement.TryGetProperty(property, out JsonElement propertyValue))
        {
            return propertyValue.GetString();
        }

        throw new JsonException($"Could not get property as string with name: '{property} from JSON.'");
    }
    
    
    private double GetDoubleProperty(JsonElement jsonElement, string property)
    {
        if (jsonElement.TryGetProperty(property, out JsonElement propertyValue))
        {
            return propertyValue.GetDouble();
        }

        throw new JsonException($"Could not get property as double with name: '{property} from JSON.'");
    }


}