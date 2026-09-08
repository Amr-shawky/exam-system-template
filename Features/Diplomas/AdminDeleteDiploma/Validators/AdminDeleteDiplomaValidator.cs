using FluentValidation;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Validators;

public class AdminDeleteDiplomaValidator : AbstractValidator<AdminDeleteDiplomaCommand>
{
    public AdminDeleteDiplomaValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Diploma Id is required.");
    }
}
