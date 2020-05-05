using Microservice.Logic.Orders.EventPublishers;
using Microservice.Logic.Orders.Events;
using Microservice.Logic.Orders.EventSubscriptionHandlers;
using Microservice.RabbitMessageBrokerHelpers.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Api.Configuration
{
    public static class MessageBrokerCollectionExtensions
    {
        public static IServiceCollection AddMessageBrokerCustomSubscriptions(this IServiceCollection services)
        {
            services
                .AddMessageBrokerSubscriptions(x =>
                   x
                       .UsePool("Orders")
                       .Subscribe<OrderPlacedSubscriptionEvent, OrderPlacedEventSubscriptionHandler>("OrderPlaced")
                )
                ;

            return services;
        }

        public static IServiceCollection AddMessageBrokerCustomPublishers(this IServiceCollection services)
        {
            services
                .AddTransient<IOrderCreatedEventPublisher, OrderCreatedEventPublisher>()
                .AddTransient<IOrderPatchedEventPublisher, OrderPatchedEventPublisher>()
                .AddTransient<IOrderPlacedEventPublisher, OrderPlacedEventPublisher>()
                ;

            return services;
        }
    }
}
