namespace backend.Services.SunriseSunsetProvider;

public class SunriseSunsetProvider : ISunriseSunsetProvider
{
    public async Task<string> GetSunriseSunset(double lat, double lon, DateTime date)
    {
        var url =
            $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date.ToString("yyyy-MM-dd")}";
        using var client = new HttpClient();
        var sunInfoData = await client.GetStringAsync(url);
        return sunInfoData;
    }
}