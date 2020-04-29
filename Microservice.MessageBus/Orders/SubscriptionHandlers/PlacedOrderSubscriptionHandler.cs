using MediatR;
using Microservice.MessageBus.Orders.Events;
using Microservice.RabbitMessageBrokerHelpers.Handlers;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.MessageBus.Orders.SubscriptionHandlers
{
    public class PlacedOrderSubscriptionHandler : MessageBrokerSubscriptionHandler<OrderPlacedSubscriptionEvent>
    {
        private readonly IMediator _mediator;
        public PlacedOrderSubscriptionHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task Handle(OrderPlacedSubscriptionEvent eventModel, CancellationToken cancellationToken)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(eventModel.Id), cancellationToken);

            if (order == null)
                return;

            // Do some custom operation here
        }
    }
}
