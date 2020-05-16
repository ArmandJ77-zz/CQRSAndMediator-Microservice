using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Db.Configuration
{
    public static class DatabaseServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string dbConnectionString)
        {
            services
                .AddDbContext<MicroserviceDbContext>(o => o.UseNpgsql(dbConnectionString));
            return services;
        }
    }
}
