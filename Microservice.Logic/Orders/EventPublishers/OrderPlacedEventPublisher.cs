using Microservice.Logic.Orders.Events;
using Microservice.RabbitMessageBrokerHelpers.Handlers;
using Microservice.RabbitMQMessageBrokerExtension;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.EventPublishers
{
    public class OrderPlacedEventPublisher : EventPublisher<OrderPlacedEvent>, IOrderPlacedEventPublisher
    {
        private IRabbitMessageBrokerClient BrokerClient { get; }

        public OrderPlacedEventPublisher(IRabbitMessageBrokerClient brokerClient)
        {
            BrokerClient = brokerClient;
        }

        public override async Task Publish(OrderPlacedEvent orderPlacedEvent)
            => await BrokerClient.Publish("OrderPlacedUpdated", orderPlacedEvent);
    }
}
