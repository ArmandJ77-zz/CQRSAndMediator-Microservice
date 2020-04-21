using Microservice.RabbitMessageBrokerHelpers.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microservice.RabbitMessageBrokerHelpers.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageBrokerSubscriptions(this IServiceCollection services,
            Action<MessageBrokerSubscriptionsConfigurationBuilder> configure)
        {
            return services;
        }
    }
}
