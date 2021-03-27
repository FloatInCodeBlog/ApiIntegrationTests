using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebHostProject.Services.Weather;
using WebHostProject.Tests.Mocks;

namespace WebHostProject.Tests.Factory
{
    public class TestWebHostProjectFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Test")
                .ConfigureTestServices(services =>
            {
                services.Replace(ServiceDescriptor.Transient<IWeatherService, WeatherServiceMock>());
            });
        }
    }
}