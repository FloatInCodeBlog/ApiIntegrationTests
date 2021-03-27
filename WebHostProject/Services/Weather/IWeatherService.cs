using System.Collections.Generic;

namespace WebHostProject.Services.Weather
{
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> Get();
    }
}