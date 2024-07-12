using backend.Models;
using backend.Repositories.CityRepository;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _cityRepository;
        private readonly ILogger<CityController> _logger;

        public CityController(ILogger<CityController> logger, ICityRepository cityRepository)
        {
            _logger = logger;
            _cityRepository = cityRepository;
        }

        [HttpGet("{city}")]
        public async Task<ActionResult<City>> GetCity(string city)
        {
            try
            {
                City c = await _cityRepository.GetCity(city);
                return Ok(c);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        } 

    }
}
