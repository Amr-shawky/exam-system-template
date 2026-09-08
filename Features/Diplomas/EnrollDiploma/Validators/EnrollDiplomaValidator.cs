using FluentValidation;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;

namespace exam_system.Features.Diplomas.EnrollDiploma.Validators;

public class EnrollDiplomaValidator : AbstractValidator<EnrollDiplomaCommand>
{
    public EnrollDiplomaValidator()
    {
        RuleFor(x => x.DiplomaId)
            .NotEmpty()
            .WithMessage("Diploma Id is required.");

        RuleFor(x => x.StudentId)
            .NotEmpty()
            .WithMessage("Student Id is required.");
    }
}
