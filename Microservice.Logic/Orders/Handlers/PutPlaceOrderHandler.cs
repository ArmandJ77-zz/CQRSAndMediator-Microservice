using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.Responses;

namespace Microservice.Logic.Orders.Handlers
{
    public class PutPlaceOrderHandler: IRequestHandler<PutPlacedOrderCommand, OrderResponse>
    {
        private readonly IMediator _mediator;

        public PutPlaceOrderHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<OrderResponse> Handle(PutPlacedOrderCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
