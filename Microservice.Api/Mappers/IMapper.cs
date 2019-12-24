using Microservice.Api.Database.EntityModels;
using Microservice.Api.Model;
using Microservice.Api.Responses;
using System.Collections.Generic;

namespace Microservice.Api.Mappers
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
