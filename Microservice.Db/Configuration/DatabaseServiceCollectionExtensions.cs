using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Db.Configuration
{
    public static class DatabaseServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string dbConnectionString)
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
