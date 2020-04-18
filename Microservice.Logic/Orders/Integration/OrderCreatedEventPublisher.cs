using Microservice.Logic.Orders.Events;
using Microservice.RabbitMessageBroker;
using System.Threading.Tasks;

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
