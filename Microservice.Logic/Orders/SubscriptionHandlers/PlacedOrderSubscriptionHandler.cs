using MediatR;
using Microservice.Logic.Orders.Events;
using Microservice.Logic.Orders.Queries;
using Microservice.RabbitMessageBrokerHelpers.Handlers;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.SubscriptionHandlers
{
    public class PlacedOrderSubscriptionHandler : MessageBrokerSubscriptionHandler<OrderPlacedSubscriptionEvent>
    {
        private readonly IMediator _mediator;
        public PlacedOrderSubscriptionHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task Handle(OrderPlacedSubscriptionEvent eventModel)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(eventModel.Id));

            if (order == null)
                return;

            // Do some custom operation here
        }
    }
}
