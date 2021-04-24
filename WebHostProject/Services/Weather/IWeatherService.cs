using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebHostProject.Services.Weather
{
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> Get();
        Task<IEnumerable<WeatherForecast>> GetRange(DateTime from, DateTime to);
        Task AddRange(IEnumerable<WeatherForecastCreate> create);
    }
}