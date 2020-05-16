namespace Microservice.Logic.Orders.Events
{
    public class OrderPlacedSubscriptionEvent
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public int Quantity { get; set; }
    }
}
