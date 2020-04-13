using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.RabbitMessageBroker.Configuration
{
    public static class RabbitMessageBrokerClientConfiguration
    {
        public static IServiceCollection AddRabbitMqMessageBroker(this IServiceCollection container, IConfigurationSection configuration)
        {
            container
                .Configure<RabbitMessageBrokerSettings>(configuration)
                ;
            
            return container
                .AddSingleton<IRabbitMessageBrokerClient, RabbitMessageBrokerClient>();
        }
    }
}
