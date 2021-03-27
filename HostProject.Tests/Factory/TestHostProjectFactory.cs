using HostProject.Services.Weather;
using HostProject.Tests.Mocks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace HostProject.Tests.Factory
{
    public class TestHostProjectFactory : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder
                .UseEnvironment("Test")
                .ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Transient<IWeatherService, WeatherServiceMock>());
            });

            return base.CreateHost(builder);
        }
    }
}