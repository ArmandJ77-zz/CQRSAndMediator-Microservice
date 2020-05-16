using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Api.Configuration
{
    public static class CorsServiceCollectionExtensions
    {
        public static IServiceCollection AddCorsRules(this IServiceCollection services)
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
