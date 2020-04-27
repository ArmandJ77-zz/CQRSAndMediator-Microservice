using MediatR;
using Microservice.Logic.Orders.Messagebus.Events;
using Microservice.RabbitMessageBrokerHelpers.Handlers;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.Messagebus.SubscriptionHandlers
{
    public class OrderPlacedSubscriptionHandler : MessageBrokerSubscriptionHandler<OrderPlacedSubscriptionEvent>
    {
        private readonly IMediator _mediator;
        public OrderPlacedSubscriptionHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override Task Handle(OrderPlacedSubscriptionEvent eventModel, CancellationToken cancellationToken)
            => _mediator.Send(eventModel.ToOrderPlacedCommand(), cancellationToken);
    }
}
