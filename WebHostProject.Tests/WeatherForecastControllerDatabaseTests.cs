using Dapper;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebHostProject.Tests.Helpers;
using Xunit;

namespace WebHostProject.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class WeatherForecastControllerDatabaseTests
    {
        [Fact]
        public async Task TestWeatherForecastDatabase()
        {
            //Create host
            using var host = Host.CreateDefaultBuilder()
               .ConfigureWebHost(webBuilder =>
               {
                   webBuilder
                       .UseTestServer()
                       .UseStartup<Startup>();
               })
               .Build();
            await host.StartAsync();

            var settings = (IOptions<DatabaseSettings>)host.GetTestServer().Services.GetService(typeof(IOptions<DatabaseSettings>));
            //Before clearing database, make sure we are using test connection string.
            if (!await CanClearDb(settings))
                throw new InvalidOperationException("Test database has too many items, check connection string.");

            //Clear database
            await ClearDatabase(settings);

            //Test data
            var additionalForecasts = new List<WeatherForecastCreate>
            {
                new WeatherForecastCreate
                {
                     Date = new DateTime(2021, 04, 24),
                     TemperatureC = 28
                }
            };

            var expectedToAdd = new List<WeatherForecastCreate>
            {
                new WeatherForecastCreate
                {
                     Date = new DateTime(2021, 04, 26),
                     TemperatureC = 19
                },
                new WeatherForecastCreate
                {
                     Date = new DateTime(2021, 04, 25),
                     TemperatureC = 21
                },
                new WeatherForecastCreate
                {
                     Date = new DateTime(2021, 04, 27),
                     TemperatureC = 14
                }
            };

            var expectedToGet = expectedToAdd.Select(_ => new WeatherForecast { Date = _.Date, TemperatureC = _.TemperatureC }).ToList();

            var allForecasts = Enumerable.Concat(expectedToAdd, additionalForecasts);

            //Create test client
            var _client = host.GetTestServer().CreateClient();

            //Add new forecasts
            var createResponse = await _client.PostAsync("WeatherForecast/add",
                new StringContent(JsonConvert.SerializeObject(allForecasts), Encoding.UTF8, "application/json"));

            createResponse.EnsureSuccessStatusCode();

            //Get filtered forecasts
            var getResponse = await _client.GetAsync($"WeatherForecast/Range?from={new DateTime(2021, 04, 25):yyyy-MM-dd}&to={new DateTime(2021, 04, 27):yyyy-MM-dd}");

            getResponse.EnsureSuccessStatusCode();
            var result = await getResponse.Content.ReadAsStringAsync();

            var actual = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(result);

            //Assert results
            var comparer = new CompareLogic();

            //When we ignore collection order we must specify key which will be used to match actual with expected
            comparer.Config.IgnoreCollectionOrder = true;
            comparer.Config.CollectionMatchingSpec.Add(typeof(WeatherForecast), new List<string> { nameof(WeatherForecast.Date) });

            //Summary will be random, we don't really care about in this test. Check if it's not null.
            comparer.Config.CustomPropertyComparer<WeatherForecast>(obj => obj.Summary,
                new CustomComparer<string, string>((expected, actual) => !string.IsNullOrEmpty(actual)));

            var compareResult = comparer.Compare(expectedToGet, actual);
            Assert.True(compareResult.AreEqual, compareResult.DifferencesString);
        }

        private static async Task ClearDatabase(IOptions<DatabaseSettings> settings)
        {
            var checkpoint = new Checkpoint
            {
                TablesToIgnore = new string[] { },
                SchemasToInclude = new string[] { },
                DbAdapter = DbAdapter.Postgres
            };
            using var conn = new NpgsqlConnection(settings.Value.ConnectionString);
            await conn.OpenAsync();

            await checkpoint.Reset(conn);
        }

        private static async Task<bool> CanClearDb(IOptions<DatabaseSettings> settings)
        {
            //If db has more entries, we assume it's not test db and we should not run tests against it.
            var maxAllowedEntries = 100;

            using var conn = new NpgsqlConnection(settings.Value.ConnectionString);

            var entries = await conn.ExecuteScalarAsync<int>("select count(*) from weather_predictions.predictions");

            return entries < maxAllowedEntries;
        }
    }
}
