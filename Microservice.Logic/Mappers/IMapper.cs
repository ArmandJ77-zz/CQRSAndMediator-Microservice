using Microservice.Db.EntityModels;
using Microservice.Logic.Model;
using System.Collections.Generic;
using Microservice.Logic.Responses;

namespace Microservice.Logic.Mappers
{
    public interface IMapper
    {
        OrderResponse MapOrderEntityToOrderResponse(Order order);
        List<OrderResponse> MapOrderEntitiesToOrderResponses(List<Order> orders);
        OrderModel MapOrderEntityToModel(Order order);
        Order MapOrderModelToEntity(OrderModel order);
        OrderResponse MapOrderModelToOrderResponse(OrderModel model);
    }
}
