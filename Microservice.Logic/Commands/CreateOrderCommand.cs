using MediatR;
using Microservice.Logic.Responses;

namespace Microservice.Logic.Commands
{
    public class CreateOrderCommand : IRequest<OrderResponse>
    {
        public string Name { get; set; }

        public CreateOrderCommand(string name)
        {
            Name = name;
        }
    }
}
