using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StocksAppTests.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Test");

            builder.ConfigureAppConfiguration((context, configuration) =>
            {
                configuration.AddUserSecrets<CustomWebApplicationFactory>();
            });

            builder.ConfigureServices(services =>
            {
                var serviceDescriptor = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<StockMarketDbContext>));
                
                if (serviceDescriptor != null)
                {
                    services.Remove(serviceDescriptor);
                }

                services.AddDbContext<StockMarketDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });
            });
        }
    }
}
