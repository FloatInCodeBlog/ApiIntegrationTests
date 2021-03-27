using System.Collections.Generic;

namespace HostProject.Services.Weather
{
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> Get();
    }
}