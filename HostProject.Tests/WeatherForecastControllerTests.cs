using HostProject.Tests.Factory;
using HostProject.Tests.Mocks;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HostProject.Tests
{
    public class WeatherForecastControllerTests : IClassFixture<TestHostProjectFactory>
    {
        private HttpClient _client;

        public WeatherForecastControllerTests(TestHostProjectFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetWeatherForecast()
        {
            var result = await _client.GetStringAsync("WeatherForecast");

            var expected = WeatherServiceMock.FakeData;
            var actual = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(result);

            var comparer = new CompareLogic();
            var compareResult = comparer.Compare(expected, actual);
            Assert.True(compareResult.AreEqual, compareResult.DifferencesString);
        }

    }
}
