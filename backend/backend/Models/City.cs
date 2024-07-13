using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class City
{
    public int Id { get; init; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    public double Latitude { get; set; }
    
    [Required]
    public double Longitude { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Country { get; set; }
    public string? State { get; set; }
    
    public ICollection<SunriseSunset> SunriseSunsets { get; set; } = new List<SunriseSunset>();
    public AppUser? User { get; set; }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Country)}: {Country}";
    }
}