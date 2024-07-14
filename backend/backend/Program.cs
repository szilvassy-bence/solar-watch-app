using System.Text;
using System.Text.Json.Serialization;
using backend.Data;
using backend.Models;
using backend.Repositories.CityRepository;
using backend.Repositories.SunriseSunsetRepository;
using backend.Services;
using backend.Services.CityProvider;
using backend.Services.GlobalExceptionHandler;
using backend.Services.JsonProcessor;
using backend.Services.SunriseSunsetProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using HostingEnvironmentExtensions = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions;

var builder = WebApplication.CreateBuilder(args);

using var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
});
var logger = loggerFactory.CreateLogger<Program>();

AddEnvironmentVariables(builder.Configuration, logger);
AddServices();
ConfigureSwagger();

var connectionString = BuildConnectionString(logger);
AddDbContext(logger);

AddAuthentication();
AddIdentity();

var app = builder.Build();

app.UseStatusCodePages();
app.UseExceptionHandler();

ApplyMigrations(logger);

using (var authSeederScope = app.Services.CreateScope())
{
    var authenticationSeeder = authSeederScope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
    authenticationSeeder.AddRoles();
    authenticationSeeder.AddAdmin();
}

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// *********** CONFIGURATION PART *********** // 

void AddEnvironmentVariables(IConfigurationBuilder configBuilder, ILogger<Program> logger)
{
    var docker = Environment.GetEnvironmentVariable("DOCKER");

    if (!string.IsNullOrEmpty(docker))
    {
        logger.LogInformation("Environment loads from docker.");
    }
    else
    {
        logger.LogInformation("Environment loads with add environment variables method.");
    }
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
    builder.Services.AddSingleton<ICityProvider, CityProvider>()
        .AddProblemDetails()
        .AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddSingleton<ISunriseSunsetProvider, SunriseSunsetProvider>()
        .AddProblemDetails()
        .AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddScoped<ICityRepository, CityRepository>()
        .AddProblemDetails()
        .AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddScoped<ISunriseSunsetRepository, SunriseSunsetRepository>()
        .AddProblemDetails()
        .AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddScoped<AuthenticationSeeder>();
}

void AddDbContext(ILogger<Program> logger)
{
    
    if (!string.IsNullOrEmpty(connectionString))
    {
        logger.LogInformation("Connection string is: {connectionString}", connectionString);
    }
    else
    {
        logger.LogInformation("Connection string cannot be found.");
    }
    //string c = "Server=database,1433;Database=Solar;User Id=sa;TrustServerCertificate=true;Solar-Watch-2024;Encrypt=false;";
    builder.Services.AddDbContext<SolarContext>(options =>
        options.UseSqlServer(connectionString, sqlOption =>
        {
            sqlOption.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        }));
}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });
}

void AddAuthentication()
{
    var validIssuer = Environment.GetEnvironmentVariable("VALIDISSUER");
    var validAudience = Environment.GetEnvironmentVariable("VALIDAUDIENCE");
    var issuerSigningKey = Environment.GetEnvironmentVariable("ISSUERSIGNINGKEY");
    
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(issuerSigningKey)
                ),
            };
        });
}

string BuildConnectionString(ILogger<Program> logger)
{
    var baseConnectionString = builder.Configuration.GetConnectionString("Solar");
    var sqlPassword = Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD");
    var sqlServerHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost,1433";
    
    if (string.IsNullOrEmpty(baseConnectionString))
    {
        logger.LogInformation("Base connection string 'Solar' not found in configuration.");
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
    logger.LogInformation("Base connection string 'Solar' is: {baseConnectionString}", baseConnectionString);
    logger.LogInformation("Environment variable 'SQLSERVER_PASSWORD' is: {sqlPassword}", sqlPassword);
    logger.LogInformation("Environment variable 'DB_HOST' is: {sqlServerHost}", sqlServerHost);
    
    var conStrBuilder = new SqlConnectionStringBuilder(baseConnectionString);
    conStrBuilder.Password = sqlPassword;
    conStrBuilder.DataSource = sqlServerHost;
    var connectionString = conStrBuilder.ConnectionString;
    logger.LogInformation("SQL connection string is: {connectionString}", connectionString);
    return connectionString;
}

void AddIdentity()
{
    builder.Services
        .AddIdentityCore<AppUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<SolarContext>();
}

async void ApplyMigrations(ILogger<Program> logger)
{
    using var scope = app.Services.CreateScope();

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

            var api = Environment.GetEnvironmentVariable("OPENWEATHERAPIKEY");
            if (!string.IsNullOrEmpty(api))
            {
                logger.LogInformation("Api: {api}", api);
            }
            else
            {
                logger.LogInformation("Api not found.");
            }
            
            var sign = Environment.GetEnvironmentVariable("ISSUERSIGNINGKEY");
            if (!string.IsNullOrEmpty(sign))
            {
                logger.LogInformation("sign: {sign}", sign);
            }
            else
            {
                logger.LogInformation("sign not found.");
            }
            
                        
            var docker = Environment.GetEnvironmentVariable("DOCKER");
            if (!string.IsNullOrEmpty(docker))
            {
                logger.LogInformation("docker: {docker}", docker);
            }
            else
            {
                logger.LogInformation("docker not found.");
            }
            
            var audience = Environment.GetEnvironmentVariable("VALIDAUDIENCE");
            if (!string.IsNullOrEmpty(audience))
            {
                logger.LogInformation("audience: {audience}", audience);
            }
            else
            {
                logger.LogInformation("audience not found.");
            }
            
            var issuer = Environment.GetEnvironmentVariable("VALIDISSUER");
            if (!string.IsNullOrEmpty(issuer))
            {
                logger.LogInformation("issuer: {issuer}", issuer);
            }
            else
            {
                logger.LogInformation("issuer not found.");
            }
            
            var sqlPass = Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD");
            if (!string.IsNullOrEmpty(sqlPass))
            {
                logger.LogInformation("sqlPass: {sqlPass}", sqlPass);
            }
            else
            {
                logger.LogInformation("sqlPass not found.");
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

bool IsRunningInDocker()
{
    var dockerEnv = Environment.GetEnvironmentVariable("DOCKER");
    return dockerEnv?.ToLower() == "true";
}