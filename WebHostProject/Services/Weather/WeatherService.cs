using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebHostProject.DataAccess;
using WebHostProject.DataAccess.Weather;

namespace WebHostProject.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly WeatherDataAccess _weatherDataAccess;

        public WeatherService(WeatherDataAccess weatherDataAccess)
        {
            _weatherDataAccess = weatherDataAccess;
        }

        public async Task AddRange(IEnumerable<WeatherForecastCreate> create)
        {
            var daos = create.Select(p => new Prediction { Day = p.Date, Temperature = p.TemperatureC });

            _weatherDataAccess.Predictions.AddRange(daos);
            await _weatherDataAccess.SaveChangesAsync();
        }

        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        public async Task<IEnumerable<WeatherForecast>> GetRange(DateTime from, DateTime to)
        {
            var daos = await _weatherDataAccess.Predictions.Where(p => from <= p.Day && p.Day <= to).ToListAsync();

            var results = daos.Select(p => new WeatherForecast
            {
                Date = p.Day,
                TemperatureC = (int)p.Temperature
            });

            foreach (var result in results)
            {
                result.Summary = Summaries[result.TemperatureC % Summaries.Length];
            }

            return results;
        }
    }
}
