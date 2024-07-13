using System.Text.Json;
using backend.Repositories.CityRepository;
using backend.Services.CityProvider;
using backend.Services.JsonProcessor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
builder.Services.AddSingleton<ICityProvider, CityProvider>();
builder.Services.AddScoped<ICityRepository, CityRepository>();

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