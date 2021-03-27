using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HostProject.Tests.Factory
{
    public class HttpClientFactory
    {
        public async Task<HttpClient> GetAsync(Action<IServiceCollection> configureServices)
        {
            var host = new HostBuilder()
              .ConfigureWebHost(webBuilder =>
              {
                  webBuilder
                      .UseTestServer()
                      .UseStartup<Startup>();
              })
              .ConfigureServices(configureServices)
              .Build();
            await host.StartAsync();

            return host.GetTestServer().CreateClient();
        }
    }
}
