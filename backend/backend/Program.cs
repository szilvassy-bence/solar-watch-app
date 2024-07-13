using System.Text.Json;
using backend.Data;
using backend.Repositories.CityRepository;
using backend.Services.CityProvider;
using backend.Services.JsonProcessor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
builder.Services.AddSingleton<ICityProvider, CityProvider>();
builder.Services.AddScoped<ICityRepository, CityRepository>();

var dbHost = System.Environment.GetEnvironmentVariable("DATABASE_HOST");
// in docker we set this, otherwise default to localhost
dbHost ??= "localhost";
var dbConnectionString = $"Server={dbHost},1433;Database=Solar;User Id=sa;Password=Solar-Watch-2024;Encrypt=false;";
AddDbContext();

void AddDbContext()
{
    builder.Services.AddDbContext<SolarContext>(options =>
        options.UseSqlServer(dbConnectionString));
}

var isTestEnvironment = Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT");

// Check if the environment variable is set
if (isTestEnvironment != null)
{
    Console.WriteLine($"IS_TEST_ENVIRONMENT is set to: {isTestEnvironment}");
}
else
{
    Console.WriteLine("IS_TEST_ENVIRONMENT is not set.");
}

var app = builder.Build();

ApplyMigrations();

async void ApplyMigrations()
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    const int maxRetries = 6;
    int retries = 0;
    bool dbReady = false;

    while (!dbReady && retries < maxRetries)
    {
        try
        {
            var solarContext = scope.ServiceProvider.GetRequiredService<SolarContext>();
            
            logger.LogInformation("Try to connect and migrate with connection string: {connectionString}", dbConnectionString);

            var canConnect = await solarContext.Database.CanConnectAsync();
            if (canConnect)
            {
                logger.LogInformation("Could connect to database.");
            }
            else
            {
                logger.LogInformation("Could not connect to database.");
            }

            solarContext.Database.Migrate();

            dbReady = true;
            logger.LogInformation("Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            retries++;
            logger.LogWarning(ex,
                "Database not ready. Waiting before retrying... Attempt {Attempt} of {MaxRetries}", retries,
                maxRetries);
            await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retries))); // Exponential backoff
        }
    }

    if (!dbReady)
    {
        logger.LogError("Failed to apply migrations after {MaxRetries} attempts.", maxRetries);
    }
}

app.Logger.LogInformation(0, "The application is started.");

// Configure the HTTP request pipeline.
app.Logger.LogInformation("The app is in development: {development}", app.Environment.IsDevelopment());
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();