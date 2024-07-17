using backend.Contracts;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly SolarContext _context;

    public UserRepository(SolarContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AppUser>> GetAll()
    {
        return await _context.Users
            .ToListAsync();
    }

    public async Task<AppUser> GetByName(string userName)
    {
        var user = await _context.Users
            .Include(u => u.Favorites)
            .FirstOrDefaultAsync(u => u.UserName == userName);

        if (user is null)
        {
            throw new ArgumentException("User not found.");
        }

        return user;
    }

    private async Task<AppUser> GetById(string id)
    {
        AppUser? user = await _context.Users.FindAsync(id);
        if (user is null) throw new ArgumentException("User is not in the database.");
        return user;
    }

    public async Task DeleteById(string id)
    {
        AppUser user = await GetById(id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<CityDto>> Favorites(string userName)
    {
        AppUser user = await GetByName(userName);
        return user.Favorites.Select(c => c.AsDto());
    }

    public async Task AddFavorite(string userName, City city)
    {
        AppUser user = await GetByName(userName);
        if (user.Favorites.Contains(city)) throw new ArgumentException("City is already in favorites.");
        
        user.Favorites.Add(city);
        await _context.SaveChangesAsync();
    }
    
    public async Task RemoveFavorite(string userName, City city)
    {
        AppUser user = await GetByName(userName);
        if (!user.Favorites.Contains(city)) throw new ArgumentException("City is not in favorites.");
        user.Favorites.Remove(city);

        await _context.SaveChangesAsync();
    }
}