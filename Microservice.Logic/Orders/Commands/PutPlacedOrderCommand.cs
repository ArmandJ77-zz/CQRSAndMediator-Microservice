using MediatR;
using Microservice.Logic.Orders.Responses;

namespace Microservice.Logic.Orders.Commands
{
    public class PutPlacedOrderCommand : IRequest<OrderResponse>
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
    }
}
