namespace Microservice.Logic.Orders.Events
{
    public class OrderCreatedEvent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
