using Microservice.RabbitMessageBrokerHelpers.BackgroundJobs;
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
            var configurationBuilder = new MessageBrokerSubscriptionsConfigurationBuilder();
            configure.Invoke(configurationBuilder);

            foreach (var subscription in configurationBuilder.Subscriptions)
                services.AddTransient(subscription.Handler);

            return services
                    .AddSingleton(configurationBuilder)
                    .AddHostedService<MessageBrokerSubscriptionBackgroundJob>()
                ;
        }
    }
}
