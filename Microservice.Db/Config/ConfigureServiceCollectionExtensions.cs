using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microservice.Db.Config
{
    public static class ConfigureServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDb(this IServiceCollection services, string dbConnectionString)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var env = serviceProvider.GetService<IHostingEnvironment>();
            
            if (env.IsEnvironment("IntegrationTesting"))
            {
                services.AddEntityFrameworkInMemoryDatabase();
                services.AddDbContext<MicroserviceDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestDB");
                });
            }
            else
            {
                services.AddDbContext<MicroserviceDbContext>(o => o.UseNpgsql(dbConnectionString));

            }

            return services;
        }
    }
}
