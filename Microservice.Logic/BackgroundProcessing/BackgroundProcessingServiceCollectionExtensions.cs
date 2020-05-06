using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Logic.BackgroundProcessing
{
    public static class BackgroundProcessingServiceCollectionExtensions
    {
        public static IServiceCollection AddBackgroundProcessing(this IServiceCollection container, IConfiguration config)
        {
            container
                .Configure<JobSettings>(config.GetSection("BackgroundProcessing"))
                .AddHostedService<JobScheduler>()
                .AddTransient<HeartbeatRecurringJob>()
                .AddTransient<ReStockZeroQuantityItemsRecurringJob>()
                ;

            return container;
        }
    }
}
