using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebHostProject.Services.Weather;

namespace WebHostProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        public WeatherForecastController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _weatherService.Get();
        }

        [HttpGet("Range")]
        public async Task<IEnumerable<WeatherForecast>> GetRangeAsync([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return await _weatherService.GetRange(from, to);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddRange(
            [FromBody] IEnumerable<WeatherForecastCreate> create)
        {
            await _weatherService.AddRange(create);

            return Ok();
        }
    }
}
