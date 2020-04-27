using MediatR;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.Handlers
{
    public class PlacedOrderHandler : IRequestHandler<PlacedOrderCommand, OrderResponse>
    {

        public PlacedOrderHandler()
        {

        }
        public Task<OrderResponse> Handle(PlacedOrderCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
