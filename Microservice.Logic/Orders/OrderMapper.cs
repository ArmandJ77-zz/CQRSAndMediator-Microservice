using Microservice.Db.EntityModels;
using Microservice.Logic.Orders.Messagebus.Events;
using Microservice.Logic.Orders.Models;
using Microservice.Logic.Orders.Responses;
using Microservice.MessageBus.Orders.Events;

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

        public static OrderPlacedCommand ToOrderPlacedCommand(
            this OrderPlacedSubscriptionEvent orderPlacedSubscriptionEvent)
        {
            //TODO CONTINUE HERE
        }

        //public static OrderCreatedEvent ToCreatedEvent(this Order order)
        //{
        //    var result = new OrderCreatedEvent
        //    {
        //        Name = order.Name,
        //        Id = order.Id
        //    };

        //    return result;
        //}
    }
}
