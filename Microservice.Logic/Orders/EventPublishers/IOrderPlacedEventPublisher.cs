using System.Threading.Tasks;
using Microservice.Logic.Orders.Events;

namespace Microservice.Logic.Orders.EventPublishers
{
    public interface IOrderPlacedEventPublisher
    {
        Task Publish(OrderPlacedEvent orderPlacedEvent);
    }
}
