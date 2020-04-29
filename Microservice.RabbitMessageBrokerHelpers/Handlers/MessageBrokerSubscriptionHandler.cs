using System.Threading.Tasks;

namespace Microservice.RabbitMessageBrokerHelpers.Handlers
{
    public abstract class MessageBrokerSubscriptionHandler<TEventModel> : IMessageBrokerSubscriptionHandler where TEventModel : class
    {
        public Task HandleEventModel(object eventModel)
        {
            return Handle(eventModel as TEventModel);
        }

        public abstract Task Handle(TEventModel eventModel);
    }
}
