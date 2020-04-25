using System.Threading;
using System.Threading.Tasks;

namespace Microservice.RabbitMessageBrokerHelpers.Handlers
{
    public interface IMessageBrokerSubscriptionHandler
    {
        Task HandleEventModel(object eventModel, CancellationToken cancellationToken);
    }
}
