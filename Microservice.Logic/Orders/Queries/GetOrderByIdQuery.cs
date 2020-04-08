using MediatR;
using Microservice.Logic.Orders.Responses;

namespace Microservice.Logic.Orders.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderResponse>
    {
        public long Id { get; set; }

        public GetOrderByIdQuery(long id)
        {
            Id = id;
        }
    }
}
