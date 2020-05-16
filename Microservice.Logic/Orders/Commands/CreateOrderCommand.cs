using MediatR;
using Microservice.Logic.Orders.Responses;

namespace Microservice.Logic.Orders.Commands
{
    public class CreateOrderCommand : IRequest<OrderResponse>
    {
        public string Name { get; }
        public int Quantity { get; }

        public CreateOrderCommand(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}
