namespace backend.Models;

public class City
{
    public int Id { get; init; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? State { get; set; }
    public string Country { get; set; }
    
    public ICollection<SunriseSunset> SunriseSunsets { get; set; } = new List<SunriseSunset>();

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Country)}: {Country}";
    }
}