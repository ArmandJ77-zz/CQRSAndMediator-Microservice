using Microservice.Logic.Orders.Events;
using Microservice.RabbitMessageBrokerHelpers.Handlers;
using Microservice.RabbitMQMessageBrokerExtension;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.EventPublishers
{
    public class OrderPatchedEventPublisher : EventPublisher<OrderUpdatedEvent>,IOrderPatchedEventPublisher
    {
        private IRabbitMessageBrokerClient BrokerClient { get; }

        public OrderPatchedEventPublisher(IRabbitMessageBrokerClient brokerClient)
        {
            BrokerClient = brokerClient;
        }
        public override async Task Publish(OrderUpdatedEvent eventModel)
            => await BrokerClient.Publish("OrderUpdated", eventModel);
    }
}
