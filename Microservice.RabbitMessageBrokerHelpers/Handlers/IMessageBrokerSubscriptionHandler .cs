using System.Threading.Tasks;

namespace Microservice.RabbitMessageBrokerHelpers.Handlers
{
    public interface IMessageBrokerSubscriptionHandler
    {
        Task HandleEventModel(object eventModel);
    }
}
