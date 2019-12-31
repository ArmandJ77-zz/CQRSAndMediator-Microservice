using FluentValidation;
using Microservice.Logic.Commands;

namespace Microservice.Logic.Validators
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
