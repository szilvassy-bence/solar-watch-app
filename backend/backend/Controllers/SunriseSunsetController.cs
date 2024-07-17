using backend.Contracts;
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
    
    [HttpGet("sunrisesunsets")]
    public async Task<ActionResult<IEnumerable<SunriseSunsetDto>>> GetAllSunInfos()
    {
        IEnumerable<SunriseSunset> sunriseSunsets = await _sunsetRepository.GetAllSunriseSunsets();
        IEnumerable<SunriseSunsetDto> sunriseSunsetDtos = sunriseSunsets.Select(ss => ss.AsDto());
        return Ok(sunriseSunsetDtos);
    }
    
    [HttpGet("{city}/{date}")]
    public async Task<ActionResult<SunriseSunsetDto>> GetSunInfoByCityByDate(string city, DateTime date)
    {
        SunriseSunset sunriseSunset = await _sunsetRepository.GetSunriseSunset(city, date);
        SunriseSunsetDto sunriseSunsetDto = sunriseSunset.AsDto();
        return Ok(sunriseSunsetDto);
    }
    
    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> DeleteSunInfoById(int id)
    {
        var sunInfo = await _sunsetRepository.GetSunriseSunsetById(id);
        
        await _sunsetRepository.DeleteSunriseSunset(sunInfo);
        return NoContent();
    }
}