using System;
using System.Collections.Generic;
using WebHostProject.Services.Weather;

namespace WebHostProject.Tests.Mocks
{
    public class WeatherServiceMock : IWeatherService
    {
        public readonly static List<WeatherForecast> FakeData = new List<WeatherForecast>
            {
                new WeatherForecast
                {
                    Date = new DateTime(2021, 03, 27),
                    Summary = "Cloudy with a chanse of meatballs",
                    TemperatureC = 25
                }
            };

        public IEnumerable<WeatherForecast> Get()
        {
            return FakeData;
        }
    }
}
