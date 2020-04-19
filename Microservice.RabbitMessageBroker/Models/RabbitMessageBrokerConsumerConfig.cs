using Microservice.RabbitMessageBroker.Logger;

namespace Microservice.RabbitMessageBroker.Models
{
    public class MessageBrokerConsumerConfig
    {
        public bool UseBasicQos { get; set; } = false;
        public ushort BasicQosPrefetch { get; set; } = 1;
        public IMessageBrokerDefaultLogger SubscriptionLogger { get; set; }
    }
}
