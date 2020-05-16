using Microservice.Logic.Orders.Events;
using System.Threading.Tasks;

namespace Microservice.Logic.Orders.EventPublishers
{
    public interface IOrderPatchedEventPublisher
    {
        Task Publish(OrderUpdatedEvent eventModel);
    }
}
