using FluentValidation;

namespace FactoryAPI.Commands;

public class CreateItemValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemValidator()
    {
        RuleFor(x => x.Name).MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.Description).MinimumLength(2).MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}