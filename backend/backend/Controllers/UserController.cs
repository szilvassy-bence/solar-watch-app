using System.Security.Claims;
using backend.Contracts;
using backend.Models;
using backend.Repositories.CityRepository;
using backend.Repositories.UserRepository;
using backend.Services.CityProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;
    private readonly ICityRepository _cityRepository;

    public UserController(
        IUserRepository userRepository, 
        ILogger<UserController> logger,
        ICityRepository cityRepository)
    {
        _userRepository = userRepository;
        _logger = logger;
        _cityRepository = cityRepository;
    }
    
    [HttpGet("users"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        try
        {
            IEnumerable<AppUser> users = await _userRepository.GetAll();
            IEnumerable<UserDto> userDtos = users.Select(u => u.AsDto());
            return Ok(userDtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Could not process the request.");
        }
    }
    
    [HttpGet("profile"), Authorize(Roles = "User")]
    public async Task<ActionResult<UserDto>> Get()
    {
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(userName))
        {
            return Unauthorized();
        }
        
        AppUser user = await _userRepository.GetByName(userName);
        return Ok(user.AsDto());
    }

    [HttpDelete("{id}/delete"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        await _userRepository.DeleteById(id);
        return NoContent();
    }
    
    [HttpGet("favorites"), Authorize(Roles = "User")]
    public async Task<ActionResult<IEnumerable<CityDto>>> Favorites()
    {
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(userName))
        {
            return Unauthorized();
        }
        
        IEnumerable<CityDto> cities = await _userRepository.Favorites(userName);
        return Ok(cities);
    }

    [HttpPatch("{id}/favorites"), Authorize(Roles = "User")]
    public async Task<IActionResult> AddFavorite(int id)
    {
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(userName))
        {
            return Unauthorized();
        }

        City city = await _cityRepository.GetCityById(id);
        await _userRepository.AddFavorite(userName, city);

        return NoContent();
    }
}