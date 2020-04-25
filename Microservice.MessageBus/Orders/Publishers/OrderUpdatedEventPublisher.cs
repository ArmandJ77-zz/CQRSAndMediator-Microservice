//using System.Threading.Tasks;
//using Microservice.RabbitMessageBroker;

//namespace Microservice.MessageBus.Orders.Publishers
//{
//    public class OrderUpdatedEventPublisher: IMessageBusPublisher<OrderUpdatedEventPublisher>
//    {
//        private IRabbitMessageBrokerClient BrokerClient { get; }

//        public OrderUpdatedEventPublisher(IRabbitMessageBrokerClient brokerClient)
//        {
//            BrokerClient = brokerClient;
//        }

//        public async Task Publish<TMessage>(TMessage message)
//        {
//            await BrokerClient.Publish("OrderUpdated", message);
//        }
//    }
//}
