using backend.Models;

namespace backend.Repositories.UserRepository;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetAll();
}