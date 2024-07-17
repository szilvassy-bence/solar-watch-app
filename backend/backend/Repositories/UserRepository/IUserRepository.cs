using backend.Contracts;
using backend.Models;

namespace backend.Repositories.UserRepository;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetAll();
    Task<AppUser> GetByName(string userName);
    Task DeleteById(string id);
    Task<IEnumerable<CityDto>> Favorites(string userName);
    Task AddFavorite(string userName, City city);
}