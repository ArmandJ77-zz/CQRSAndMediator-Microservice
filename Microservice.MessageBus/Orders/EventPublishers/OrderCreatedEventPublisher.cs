using Microservice.RabbitMessageBroker;
using System.Threading.Tasks;

namespace Microservice.MessageBus.Orders.EventPublishers
{
    public class OrderCreatedEventPublisher : IMessageBusPublisher<OrderCreatedEventPublisher>
    {
        private IRabbitMessageBrokerClient BrokerClient { get; }

        public OrderCreatedEventPublisher(IRabbitMessageBrokerClient brokerClient)
        {
            BrokerClient = brokerClient;
        }

        public async Task Publish<TMessage>(TMessage createdEvent)
            => await BrokerClient.Publish("OrderCreated", createdEvent);
    }
}
