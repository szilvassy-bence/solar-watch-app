using backend.Models;

namespace backend.Contracts;

public record UserDto(
    string Id,
    string UserName,
    string Email,
    IEnumerable<City> Favorites);