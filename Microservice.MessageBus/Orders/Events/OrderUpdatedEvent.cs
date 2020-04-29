namespace Microservice.MessageBus.Orders.Events
{
    public class OrderUpdatedEvent
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
