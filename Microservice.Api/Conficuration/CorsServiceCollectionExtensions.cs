using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Api.Conficuration
{
    public static class CorsServiceCollectionExtensions
    {
        public static IServiceCollection AddCors(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddCors(options => options.AddPolicy("Default", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            return services;
        }
    }
}
