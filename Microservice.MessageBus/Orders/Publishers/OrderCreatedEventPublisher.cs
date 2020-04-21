using System.Threading.Tasks;
using Microservice.RabbitMessageBroker;

namespace Microservice.MessageBus.Orders.Publishers
{
    public class OrderCreatedEventPublisher: IMessageBusPublisher<OrderCreatedEventPublisher>
    {
        private IRabbitMessageBrokerClient BrokerClient { get; }

        public OrderCreatedEventPublisher(IRabbitMessageBrokerClient brokerClient)
        {
            BrokerClient = brokerClient;
        }

        public async Task Publish<TMessage>(TMessage createdEvent)
        {
            await BrokerClient.Publish("OrderCreated", createdEvent);
        }
    }
}
