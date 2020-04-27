using MediatR;
using Microservice.Logic.Orders.Responses;

namespace Microservice.Logic.Orders.Commands
{
    public class PlacedOrderCommand : IRequest<OrderResponse>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
