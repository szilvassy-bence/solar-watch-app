using System.Text.Json;
using backend.Models;
using backend.Services.JsonProcessor;

namespace unit_test;

public class JsonProcessorTest
{
    private IJsonProcessor _jsonProcessor;

    [SetUp]
    public void Setup()
    {
        _jsonProcessor = new JsonProcessor();
    }
    
    [Test]
    public void ProcessCityJsonReturnsCorrectCity()
    {
        // Arrange
        string dataWithState = "[{\"name\":\"London\",\"lat\":51.5073219,\"lon\":-0.1276474,\"country\":\"GB\",\"state\":\"England\"}]";
        string dataWithoutState = "[{\"name\":\"London\",\"lat\":51.5073219,\"lon\":-0.1276474,\"country\":\"GB\"}]";
        
        // Act
        City cityWithState = _jsonProcessor.ProcessCity(dataWithState);
        City cityWithoutState = _jsonProcessor.ProcessCity(dataWithoutState);
        
        // Assert
        Assert.IsNotNull(cityWithState);
        Assert.That(cityWithoutState.Latitude, Is.EqualTo(51.5073219));
        Assert.IsNull(cityWithoutState.State);
        Assert.IsNotNull(cityWithState.State);
        Assert.That(cityWithState.State, Is.EqualTo("England"));
    }

    [Test]
    public void EmptyStringThrowsException()
    {
        // Arrange
        string data = "";
        
        // Act
        // Assert
        Assert.Throws<JsonException>(() => _jsonProcessor.ProcessCity(data));
    }
    
    [Test]
    public void IncompleteStringThrowsException()
    {
        // Arrange
        string data = "[{\"name\":\"London\",\"lon\":-0.1276474,\"country\":\"GB\",\"state\":\"England\"}]";
        
        // Act
        // Assert
        Assert.Throws<JsonException>(() => _jsonProcessor.ProcessCity(data));
    }
    
    [Test]
    public void ProcessSunriseSunsetJsonReturnsCorrectCity()
    {
        // Arrange
        string sunriseSunsetString = "{\"results\":{\"sunrise\":\"7:27:02 AM\",\"sunset\":\"5:05:55 PM\"}}";
        
        // Act
        SunriseSunset sunriseSunset = _jsonProcessor.ProcessSunriseSunset(sunriseSunsetString, new DateTime());
        
        // Assert
        Assert.IsNotNull(sunriseSunset);
        Assert.IsInstanceOf<DateTime>(sunriseSunset.Sunrise);
    }
    
    [Test]
    public void EmptySunriseSunsetStringThrowsException()
    {
        // Arrange
        string data = "";
        
        // Act
        // Assert
        Assert.Throws<JsonException>(() => _jsonProcessor.ProcessSunriseSunset(data, new DateTime()));
    }
    
    [Test]
    public void IncompleteSunriseSunsetStringThrowsException()
    {
        // Arrange
        string data = "{\"results\":{\"sunrise\":\"7:27:02 AM\"}}";
        
        // Act
        // Assert
        Assert.Throws<JsonException>(() => _jsonProcessor.ProcessSunriseSunset(data, new DateTime()));
    }
}