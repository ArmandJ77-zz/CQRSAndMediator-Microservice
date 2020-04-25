using System.Threading;
using System.Threading.Tasks;

namespace Microservice.RabbitMessageBrokerHelpers.Handlers
{
    public abstract class MessageBrokerSubscriptionHandler<TEventModel> : IMessageBrokerSubscriptionHandler where TEventModel : class
    {
        public Task HandleEventModel(object eventModel, CancellationToken cancellationToken)
        {
            return Handle(eventModel as TEventModel, cancellationToken);
        }

        public abstract Task Handle(TEventModel eventModel, CancellationToken cancellationToken);
    }
}
