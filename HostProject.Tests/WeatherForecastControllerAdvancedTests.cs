using HostProject.Services.Weather;
using HostProject.Tests.Mocks;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HostProject.Tests
{
    public class WeatherForecastControllerAdvancedTests
    {
        [Fact]
        public async Task GetWeatherForecastMoq()
        {
            var mock = new Mock<IWeatherService>();

            using var host = new HostBuilder()
               .ConfigureWebHost(webBuilder =>
               {
                   webBuilder
                       .UseTestServer()
                       .UseStartup<Startup>();
               })
               .ConfigureServices(services =>
               {
                   services.Replace(ServiceDescriptor.Transient(_ => mock.Object));
               })
               .Build();
            await host.StartAsync();

            mock.Setup(_ => _.Get()).Returns(WeatherServiceMock.FakeData);

            var _client = host.GetTestServer().CreateClient();

            var result = await _client.GetStringAsync("WeatherForecast");

            var expected = WeatherServiceMock.FakeData;
            var actual = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(result);

            var comparer = new CompareLogic();
            var compareResult = comparer.Compare(expected, actual);
            Assert.True(compareResult.AreEqual, compareResult.DifferencesString);
        }

    }
}
