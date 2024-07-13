using Microsoft.AspNetCore.Identity;

namespace backend.Models;

public class AppUser : IdentityUser
{
    public ICollection<City> Favorites { get; set; } = new List<City>();
}