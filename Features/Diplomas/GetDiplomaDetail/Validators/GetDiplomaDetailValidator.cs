using FluentValidation;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Validators;

public class GetDiplomaDetailValidator : AbstractValidator<GetDiplomaDetailQuery>
{
    public GetDiplomaDetailValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Diploma Id is required.");
    }
}
