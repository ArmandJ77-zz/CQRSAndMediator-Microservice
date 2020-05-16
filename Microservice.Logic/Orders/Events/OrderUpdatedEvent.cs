namespace Microservice.Logic.Orders.Events
{
    public class OrderUpdatedEvent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
