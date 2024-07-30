using backend.Data;
using integration_test.helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace integration_test;

public class SolarWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureTestServices(services =>
        {
            Environment.SetEnvironmentVariable("IS_TEST_ENVIRONMENT", "true");
            Environment.SetEnvironmentVariable("SQLSERVER_PASSWORD", "Solar-Watch-2024");
            Environment.SetEnvironmentVariable("ISSUERSIGNINGKEY", "!SomethingSecret!!Solar!TOPsecret!");
            Environment.SetEnvironmentVariable("OPEN_WEATHER_API_KEY", "734d904cb1e70969120b4f75e9fe980a");

            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<SolarContext>));

            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            services.AddDbContext<SolarContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
                options.UseInternalServiceProvider(serviceProvider);
            });

            services.AddTransient<Seeder>();

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();

            using var solarWatchContext = scope.ServiceProvider.GetRequiredService<SolarContext>();
            solarWatchContext.Database.EnsureDeleted();
            solarWatchContext.Database.EnsureCreated();

            var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
            seeder.ReinitializeSolarWatchDbForTests();
            seeder.ReinitializeIdentityUserDbForTests().Wait();
        });

        builder.UseEnvironment("Development");
 
    }

}