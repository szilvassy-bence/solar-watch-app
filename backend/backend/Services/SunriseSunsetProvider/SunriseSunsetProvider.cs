namespace backend.Services.SunriseSunsetProvider;

public class SunriseSunsetProvider : ISunriseSunsetProvider
{
    public async Task<string> GetSunriseSunset(double lat, double lon, DateTime date)
    {
        var url =
            $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date.ToString("yyyy-MM-dd")}";
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();

            if (json.Trim() == "[]")
            {
                throw new ArgumentException("No sunrise sunset found for the given request.");
            }

            return json;
        }
        else
        {
            throw new Exception($"Failed to retrieve data. Status code: {response.StatusCode}");
        }
    }
}