using System.Threading.Tasks;

namespace Microservice.MessageBus
{
    public interface IMessageBusPublisher<T>
    {
        Task Publish<TMessage>(TMessage message);
    }
}
