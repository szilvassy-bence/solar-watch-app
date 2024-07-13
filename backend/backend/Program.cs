using System.Text.Json.Serialization;
using backend.Data;
using backend.Repositories.CityRepository;
using backend.Repositories.SunriseSunsetRepository;
using backend.Services;
using backend.Services.CityProvider;
using backend.Services.JsonProcessor;
using backend.Services.SunriseSunsetProvider;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

AddEnvironmentVariables(builder.Configuration);
AddServices();

var connectionString = BuildConnectionString();
AddDbContext();

var app = builder.Build();

ApplyMigrations();

app.Logger.LogInformation("The application is started.");

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

// *********** CONFIGURATION PART *********** // 

void AddEnvironmentVariables(IConfigurationBuilder configBuilder)
{
    if (!IsRunningInDocker())
    {
        var parent = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
        var dotenv = Path.Combine(parent, ".env");
        Console.WriteLine(dotenv);
        DotEnv.Load(dotenv);

        configBuilder.AddEnvironmentVariables().Build();
    }
}


void AddServices()
{
    builder.Services.AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
    builder.Services.AddSingleton<ICityProvider, CityProvider>();
    builder.Services.AddSingleton<ISunriseSunsetProvider, SunriseSunsetProvider>();
    builder.Services.AddScoped<ICityRepository, CityRepository>();
    builder.Services.AddScoped<ISunriseSunsetRepository, SunriseSunsetRepository>();
}

bool IsRunningInDocker()
{
    var dockerEnv = Environment.GetEnvironmentVariable("DOCKER");
    return dockerEnv?.ToLower() == "true";
}

void AddDbContext()
{
    builder.Services.AddDbContext<SolarContext>(options =>
        options.UseSqlServer(connectionString));
}

string BuildConnectionString()
{
    using var loggerFactory = LoggerFactory.Create(logging =>
    {
        logging.AddConsole();
    });
    var logger = loggerFactory.CreateLogger<Program>();
    
    var baseConnectionString = builder.Configuration.GetConnectionString("Solar");
    var sqlPassword = Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD");
    var sqlServerHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost,1433";
    
    if (string.IsNullOrEmpty(baseConnectionString))
    {
        logger.LogInformation("Connection string 'Solar' not found in configuration.");
        throw new InvalidOperationException("Connection string 'Solar' not found in configuration.");
    }

    if (string.IsNullOrEmpty(sqlPassword))
    {
        logger.LogInformation("Environment variable 'SQLSERVER_PASSWORD' not found in configuration.");
        throw new InvalidOperationException("Environment variable 'SQLSERVER_PASSWORD' not set.");
    }

    if (string.IsNullOrEmpty(sqlServerHost))
    {
        logger.LogInformation("Connection string 'SW_DB_HOST' not found in configuration.");
        throw new InvalidOperationException("Environment variable 'DB_HOST' not set.");
    }
    logger.LogInformation("Connection string 'Solar' is: {baseConnectionString}", baseConnectionString);
    logger.LogInformation("Environment variable 'SQLSERVER_PASSWORD' is: {sqlPassword}", sqlPassword);
    logger.LogInformation("Environment variable 'DB_HOST' is: {sqlServerHost}", sqlServerHost);
    
    var conStrBuilder = new SqlConnectionStringBuilder(baseConnectionString);
    conStrBuilder.Password = sqlPassword;
    conStrBuilder.DataSource = sqlServerHost;
    var connectionString = conStrBuilder.ConnectionString;
    logger.LogInformation("SQL connection string is: {connectionString}", connectionString);
    return connectionString;
}

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
            
            logger.LogInformation("Try to connect and migrate with connection string: {connectionString}", connectionString);

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