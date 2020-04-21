using Microservice.RabbitMessageBrokerHelpers.Handlers;
using Microservice.RabbitMessageBrokerHelpers.Models;
using System.Collections.Generic;

namespace Microservice.RabbitMessageBrokerHelpers.Builders
{
    public class MessageBrokerSubscriptionsConfigurationBuilder
    {
        public List<MessageBrokerSubscription> Subscriptions;
        public string Pool;

        public MessageBrokerSubscriptionsConfigurationBuilder UsePool(string pool)
        {
            Pool = pool;
            return this;
        }

        public MessageBrokerSubscriptionsConfigurationBuilder Subscribe<TEventModel, THandler>(string topic)
            where THandler : MessageBrokerSubscriptionHandler<TEventModel>
            where TEventModel : class
        {
            Subscriptions.Add(new MessageBrokerSubscription
            {
                Topic = topic,
                Handler = typeof(THandler),
                EventModel = typeof(TEventModel)
            });
            return this;
        }

        public MessageBrokerSubscriptionsConfigurationBuilder Subscribe<THandler>(string topic)
            where THandler : MessageBrokerSubscriptionHandler<object>
        {
            return Subscribe<object, THandler>(topic);
        }
    }
}
