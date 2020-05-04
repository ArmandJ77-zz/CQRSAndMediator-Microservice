using MediatR;
using Microservice.Logic.Orders.Events;
using Microservice.RabbitMessageBrokerHelpers.Handlers;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.EventSubscriptions
{
    public class OrderPlacedEventSubscription : MessageBrokerSubscriptionHandler<OrderPlacedSubscriptionEvent>
    {
        private readonly IMediator _mediator;

        public OrderPlacedEventSubscription(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task Handle(OrderPlacedSubscriptionEvent eventModel)
        {
            await _mediator.Send(eventModel.ToPutPlacedOrderCommand());
        }
    }
}
