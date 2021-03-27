using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebHostProject.Tests.Factory;
using WebHostProject.Tests.Mocks;
using Xunit;

namespace WebHostProject.Tests
{
    public class WeatherForecastControllerTests : IClassFixture<TestWebHostProjectFactory>
    {
        private HttpClient _client;

        public WeatherForecastControllerTests(TestWebHostProjectFactory factory)
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
