using Microservice.Db.EntityModels;
using Microservice.Logic.Model;
using System.Collections.Generic;
using System.Linq;
using Microservice.Logic.Responses;

namespace Microservice.Logic.Mappers
{
    public class Mapper : IMapper
    {
        public OrderResponse MapOrderEntityToOrderResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                Name = order.Name
            };
        }

        public OrderModel MapOrderEntityToModel(Order order)
        {
            return new OrderModel
            {
                Id = order.Id,
                Name = order.Name
            };
        }

        public Order MapOrderModelToEntity(OrderModel order)
        {
            return new Order
            {
                Id = order.Id,
                Name = order.Name
            };
        }

        public OrderResponse MapOrderModelToOrderResponse(OrderModel model)
        {
            return new OrderResponse
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public List<OrderResponse> MapOrderEntitiesToOrderResponses(List<Order> orders)
        {
            return orders.Select(MapOrderEntityToOrderResponse).ToList();
        }
    }
}
