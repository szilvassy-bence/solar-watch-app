using Microsoft.AspNetCore.Identity;

namespace backend.Services.Authentication;

public interface ITokenService
{
    string CreateToken(IdentityUser user, string role);
}