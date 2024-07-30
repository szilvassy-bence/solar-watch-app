using System.Text;
using System.Text.Json;
using backend.Contracts;

namespace integration_test.helpers;

public class Helper
{
    public static async Task<string> LoginUser(HttpClient client)
    {
        var loginRequest = new AuthRequest("user@user", "password");
        var loginJson = JsonSerializer.Serialize(loginRequest);
        var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync("/api/Auth/Login", loginContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonOption = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, jsonOption);
            return authResponse.Token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    } 
    
    public static async Task<string> LoginAdmin(HttpClient client)
    {
        var loginRequest = new AuthRequest("admin@admin", "password");
        var loginJson = JsonSerializer.Serialize(loginRequest);
        var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync("/api/Auth/Login", loginContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonOption = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, jsonOption);
            return authResponse.Token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    } 
}