using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebHostProject.Services.Weather
{
    public class WeatherService : IWeatherService
    {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IOptions<DatabaseSettings> _options;

        public WeatherService(IOptions<DatabaseSettings> options)
        {
            _options = options;
        }

        public async Task AddRange(IEnumerable<WeatherForecastCreate> create)
        {
            using var conn = new NpgsqlConnection(_options.Value.ConnectionString);

            await conn.ExecuteAsync("INSERT INTO weather_predictions.predictions VALUES (@day, @temperature)",
                create.Select(_ => new { day = _.Date, temperature = _.TemperatureC }).ToList()
                );
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
            using var conn = new NpgsqlConnection(_options.Value.ConnectionString);

            var results = await conn.QueryAsync<WeatherForecast>(@"select day as date, temperature as temperatureC 
                FROM weather_predictions.predictions 
                where @from <= day and day <= @to
                order by day",
                  new { from, to }
                  );

            foreach (var result in results)
            {
                result.Summary = Summaries[result.TemperatureC % Summaries.Length];
            }

            return results;
        }
    }
}
