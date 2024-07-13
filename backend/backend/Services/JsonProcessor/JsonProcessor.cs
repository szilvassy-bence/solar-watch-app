using System.Globalization;
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
        double lon = GetDoubleProperty(jsonElement, "lon");

        double lat = GetDoubleProperty(jsonElement, "lat");
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
    
    public SunriseSunset ProcessSunriseSunset(string json, DateTime date)
    {
        if (string.IsNullOrEmpty(json))
        {
            throw new JsonException("Input json data cannot be empty.");
        }
        JsonDocument jd = JsonDocument.Parse(json);
        
        JsonElement result = jd.RootElement.GetProperty("results");

        DateTime sunriseDate = ConvertToDateTime(GetStringProperty(result, "sunrise"), date);
        DateTime sunsetDate = ConvertToDateTime(GetStringProperty(result, "sunset"), date);
        
        return new SunriseSunset { Sunrise = sunriseDate, Sunset = sunsetDate };
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
    
    private DateTime ConvertToDateTime(string time, DateTime date)
    {
        var format = "h:mm:ss tt";

        var combinedDateTimeString = $"{date.ToString("MM/dd/yyyy")} {time}";

        DateTime result;
        if (DateTime.TryParseExact(combinedDateTimeString, "MM/dd/yyyy " + format, null,
                DateTimeStyles.None, out result))
            return result;
        throw new FormatException("Failed to parse the time string.");
    }


}