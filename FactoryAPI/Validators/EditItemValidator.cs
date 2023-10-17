using FluentValidation;

namespace FactoryAPI.Commands;

public class EditItemValidator : AbstractValidator<EditItemCommand>
{
    public EditItemValidator()
    {
        RuleFor(x => x.Name).MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.Description).MinimumLength(2).MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}