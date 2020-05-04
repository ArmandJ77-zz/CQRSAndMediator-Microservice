using Microservice.Logic.Orders.Events;
using Microservice.RabbitMessageBroker;
using Microservice.RabbitMessageBrokerHelpers.Handlers;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.EventPublishers
{
    public class OrderUpdatedEventPublisher : EventPublisher<OrderUpdatedEvent>,IOrderUpdatedEventPublisher
    {
        private IRabbitMessageBrokerClient BrokerClient { get; }

        public OrderUpdatedEventPublisher(IRabbitMessageBrokerClient brokerClient)
        {
            BrokerClient = brokerClient;
        }
        public override async Task Publish(OrderUpdatedEvent eventModel)
            => await BrokerClient.Publish("OrderUpdated", eventModel);
    }
}
