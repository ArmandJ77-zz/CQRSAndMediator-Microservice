using FluentValidation;
using Microservice.Logic.Orders.Commands;

namespace Microservice.Logic.Orders.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
