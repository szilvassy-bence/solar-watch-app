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
}