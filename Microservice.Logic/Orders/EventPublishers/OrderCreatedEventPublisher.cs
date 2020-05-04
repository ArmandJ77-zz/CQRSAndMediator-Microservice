using Microservice.Logic.Orders.Events;
using Microservice.RabbitMessageBroker;
using Microservice.RabbitMessageBrokerHelpers.Handlers;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.EventPublishers
{
    public class OrderCreatedEventPublisher : EventPublisher<OrderCreatedEvent>
    {
        private IRabbitMessageBrokerClient BrokerClient { get; }

        public OrderCreatedEventPublisher(IRabbitMessageBrokerClient brokerClient)
        {
            BrokerClient = brokerClient;
        }

        public override async Task Publish(OrderCreatedEvent eventModel)
            => await BrokerClient.Publish("OrderCreated", eventModel);
    }
}
