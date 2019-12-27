using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Db.Config
{
    public static class ConfigureServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDb(this IServiceCollection container, string dbConnectionString)
        {
            return container
                    .AddDbContext<MicroserviceDbContext>(o => o.UseNpgsql(dbConnectionString))
                ;
        }
    }
}
