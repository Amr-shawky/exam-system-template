using FluentValidation;
using exam_system.Features.Diplomas.GetStudentDashboard.Queries;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Validators;

public class GetStudentDashboardValidator : AbstractValidator<GetStudentDashboardQuery>
{
    public GetStudentDashboardValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty()
            .WithMessage("Student Id is required.");
    }
}
