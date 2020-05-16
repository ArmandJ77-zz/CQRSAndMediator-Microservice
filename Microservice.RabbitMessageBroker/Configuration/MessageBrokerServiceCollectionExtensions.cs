using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.RabbitMessageBroker.Configuration
{
    public static class MessageBrokerServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqMessageBroker(this IServiceCollection container, IConfigurationSection configuration)
        {
            container
                .Configure<MessageBrokerSettings>(configuration)
                ;

            return container
                .AddSingleton<IRabbitMessageBrokerClient, RabbitMessageBrokerClient>();
        }
    }
}
