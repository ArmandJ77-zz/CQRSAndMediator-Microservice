using MediatR;
using Microservice.Logic.Orders.Responses;

namespace Microservice.Logic.Orders.Commands
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
