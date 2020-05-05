namespace Microservice.Logic.Orders.Events
{
    public class OrderPlacedEvent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int QuantityBeforeReduction { get; set; }
    }
}
