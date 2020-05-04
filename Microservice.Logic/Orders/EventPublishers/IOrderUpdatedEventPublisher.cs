using Microservice.Logic.Orders.Events;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.EventPublishers
{
    public interface IOrderUpdatedEventPublisher
    {
        Task Publish(OrderUpdatedEvent eventModel);
    }
}
