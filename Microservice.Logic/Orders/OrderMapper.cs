using Microservice.Db.EntityModels;
using Microservice.Logic.Orders.Commands;
using Microservice.Logic.Orders.Events;
using Microservice.Logic.Orders.Models;
using Microservice.Logic.Orders.Responses;

namespace Microservice.Logic.Orders
{
    public static class OrderMapper
    {
        public static Order ToCreateEntity(this CreateOrderCommand command)
        {
            var result =  new Order
            {
                Name = command.Name,
                Quantity = command.Quantity
            };

            return result;
        }
        public static OrderResponse ToResponse(this Order order)
        {
            var result = new OrderResponse
            {
                Id = order.Id,
                Name = order.Name,
                Quantity = order.Quantity
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

        public static PutOrderPlacedCommand ToPutPlacedOrderCommand(
            this OrderPlacedSubscriptionEvent subscriptionEvent)
        {
            var result = new PutOrderPlacedCommand
            (
                subscriptionEvent.Id,
                subscriptionEvent.PersonId,
                subscriptionEvent.Quantity
            );

            return result;
        }

        public static OrderUpdatedEvent ToUpdatedEvent(
            this Order order)
        {
            var result = new OrderUpdatedEvent
            {
                Id = order.Id,
                Name = order.Name,
                Quantity = order.Quantity,
            };

            return result;
        }

        public static OrderPlacedEvent ToOrderPlacedEvent(
            this Order order,
            int quantityBeforeReduction)
        {
            var result = new OrderPlacedEvent
            {
                Id = order.Id,
                Name = order.Name,
                Quantity = order.Quantity,
                QuantityBeforeReduction = quantityBeforeReduction
            };

            return result;
        }

        public static OrderCreatedEvent ToCreatedEvent(
            this Order order)
        {
            var result = new OrderCreatedEvent
            {
                Id = order.Id,
                Name = order.Name,
                Quantity = order.Quantity
            };

            return result;
        }
    }
}
