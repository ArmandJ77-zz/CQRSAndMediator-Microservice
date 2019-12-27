using MediatR;
using Microservice.Logic.Responses;

namespace Microservice.Logic.Queries
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
