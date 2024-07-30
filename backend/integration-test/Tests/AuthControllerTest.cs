using backend.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace integration_test.Test;

public class AuthControllerTest : IClassFixture<SolarWebAppFactory>
{
    private readonly HttpClient _httpClient;
    private readonly ITestOutputHelper _outputHelper;
    private readonly SolarWebAppFactory _factory;

    public AuthControllerTest(ITestOutputHelper outputHelper)
    {
        _factory = new SolarWebAppFactory();
        _httpClient = _factory.CreateClient();
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void SeededDataIsPresentInDatabase()
    {
        _outputHelper.WriteLine("Is test env: " + Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT"));
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SolarContext>();

        // Act 
        var users = dbContext.Users.ToList();
        foreach (var user in users)
        {
            _outputHelper.WriteLine(user.UserName);
        }

        // Assert
        Assert.NotNull(users);
        Assert.NotEmpty(users); 
    }
}