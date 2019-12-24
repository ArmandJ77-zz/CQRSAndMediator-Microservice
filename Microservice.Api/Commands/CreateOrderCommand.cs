using MediatR;
using Microservice.Api.Responses;

namespace Microservice.Api.Commands
{
    public class CreateOrderCommand : IRequest<OrderResponse>
    {
        public string Name { get; set; }
    }
}
