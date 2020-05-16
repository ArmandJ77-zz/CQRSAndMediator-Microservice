using System.Threading.Tasks;

namespace Microservice.RabbitMessageBrokerHelpers.Handlers
{
    public abstract class EventPublisher<TPublishEvent> where TPublishEvent : class
    {
        public abstract Task Publish(TPublishEvent eventModel);
    }
}
