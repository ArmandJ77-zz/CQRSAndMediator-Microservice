using System.Collections.Generic;
using MediatR;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.Queries;
using Microservice.Logic.Orders.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.Handlers
{
    public class PutPlaceOrderHandler : IRequestHandler<PutPlacedOrderCommand, OrderResponse>
    {
        private readonly IMediator _mediator;

        public PutPlaceOrderHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OrderResponse> Handle(PutPlacedOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(request.Id), cancellationToken);

            if(order == null)
                throw new KeyNotFoundException($"Unable to modify order because an entry with Id: {request.Id} could not be found");

            return null;


        }
    }
}
