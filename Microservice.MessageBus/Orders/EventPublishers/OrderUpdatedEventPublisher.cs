using Microservice.RabbitMessageBroker;
using System.Threading.Tasks;

namespace Microservice.MessageBus.Orders.EventPublishers
{
    public class OrderUpdatedEventPublisher : IMessageBusPublisher<OrderUpdatedEventPublisher>
    {
        private IRabbitMessageBrokerClient BrokerClient { get; }

        public OrderUpdatedEventPublisher(IRabbitMessageBrokerClient brokerClient)
        {
            BrokerClient = brokerClient;
        }

        public async Task Publish<TMessage>(TMessage message)
            => await BrokerClient.Publish("OrderUpdated", message);
    }
}
