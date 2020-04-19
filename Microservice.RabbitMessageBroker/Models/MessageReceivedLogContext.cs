namespace Microservice.RabbitMessageBroker.Models
{
    public class MessageReceivedLogContext
    {
        public string CorrelationId { get; set; }
        public string SubscriptionId { get; set; }
        public string Topic { get; set; }
    }
}
