using Microservice.Db.EntityModels;
using Microservice.Logic.Model;
using Microservice.Logic.Orders.Responses;

namespace Microservice.Logic.Mappers
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
    }
}
