using MediatR;
using Microservice.Api.Responses;

namespace Microservice.Api.Queries
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
