using FluentValidation;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Validators;

public class BrowseDiplomasValidator : AbstractValidator<BrowseDiplomasQuery>
{
    public BrowseDiplomasValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0)
            .WithMessage("Page index must be greater than zero.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");
    }
}
