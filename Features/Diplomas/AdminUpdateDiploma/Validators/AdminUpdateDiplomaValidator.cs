using FluentValidation;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Validators;

public class AdminUpdateDiplomaValidator : AbstractValidator<AdminUpdateDiplomaCommand>
{
    public AdminUpdateDiplomaValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Diploma Id is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 200).WithMessage("Title must be between 3 and 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
    }
}
