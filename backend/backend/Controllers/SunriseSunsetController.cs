using backend.Models;
using backend.Repositories.SunriseSunsetRepository;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SunriseSunsetController : ControllerBase
{
    private readonly ILogger<SunriseSunsetController> _logger;
    private readonly ISunriseSunsetRepository _sunsetRepository;

    public SunriseSunsetController(ILogger<SunriseSunsetController> logger, ISunriseSunsetRepository sunsetRepository)
    {
        _logger = logger;
        _sunsetRepository = sunsetRepository;
    }
    
    [HttpGet("{city}/solar/{date}")]
    public async Task<ActionResult<SunriseSunset>> GetSunInfoByCityByDate(string city, DateTime date)
    {
        return Ok(await _sunsetRepository.GetSunriseSunset(city, date));
    }
}