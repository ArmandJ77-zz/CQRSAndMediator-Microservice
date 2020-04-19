using Microservice.Logic.Orders.Events;
using System.Threading.Tasks;
using Microservice.RabbitMessageBroker;

namespace Microservice.Logic.Orders.Integration
{
    public class OrderCreatedEventPublisher : IOrderCreatedEventPublisher
    {
        private IRabbitMessageBrokerClient BrokerClient { get; }

        public OrderCreatedEventPublisher(IRabbitMessageBrokerClient brokerClient)
        {
            BrokerClient = brokerClient;
        }

        public async Task Publish(OrderCreatedEvent createdEvent)
        {
            await BrokerClient.Publish("OrderCreated", createdEvent);

        }
    }
}
