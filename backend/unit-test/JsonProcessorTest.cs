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
}