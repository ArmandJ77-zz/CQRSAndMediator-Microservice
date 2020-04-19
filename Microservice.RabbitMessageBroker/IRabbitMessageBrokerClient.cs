using System;
using System.Threading.Tasks;
using Microservice.RabbitMessageBroker.Builders;

namespace Microservice.RabbitMessageBroker
{
    public interface IRabbitMessageBrokerClient
    {
        Task<Action> Subscribe<T>(
            string topic, 
            string subscriptionId, 
            Func<T, Task> onReceive,
            Func<MessageBrokerConfigBuilder, MessageBrokerConfigBuilder> configure = null);

        Task<Action> Subscribe(
            string topic,
            string subscriptionId,
            Func<object, Task> onReceive,
            Type eventModelType,
            Func<MessageBrokerConfigBuilder, MessageBrokerConfigBuilder> config = null);

        Task<bool> Publish<T>(string topic, T message);

    }
}
