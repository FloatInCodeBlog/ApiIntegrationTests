using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weather.Common.Settings;
using WebHostProject.DataAccess.Weather;

namespace WebHostProject.DataAccess
{
    public class WeatherDataAccess : weatherContext
    {
        private readonly IOptions<DatabaseSettings> _options;

        public WeatherDataAccess(IOptions<DatabaseSettings> options)
        {
            _options = options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_options.Value.ConnectionString);
        }
    }
}
