using Microservice.Db.EntityModels;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.Messagebus.Events;
using Microservice.Logic.Orders.Models;
using Microservice.Logic.Orders.Responses;

namespace Microservice.Logic.Orders
{
    public static class OrderMapper
    {
        public static OrderResponse ToResponse(this Order order)
        {
            var result = new OrderResponse
            {
                Id = order.Id,
                Name = order.Name
            };

            return result;
        }

        public static OrderPatchModel ToPatchModal(this Order order)
        {
            var result = new OrderPatchModel
            {
                Name = order.Name
            };

            return result;
        }

        public static PlacedOrderCommand ToOrderPlacedCommand(
            this OrderPlacedSubscriptionEvent subscriptionEvent)
        {
            var result = new PlacedOrderCommand
            {
                Id = subscriptionEvent.Id
            };

            return result;
        }
    }
}
