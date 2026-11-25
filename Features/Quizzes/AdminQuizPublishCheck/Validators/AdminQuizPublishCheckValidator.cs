using FluentValidation;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Validators;

public class AdminQuizPublishCheckValidator : AbstractValidator<AdminQuizPublishCheckQuery>
{
    public AdminQuizPublishCheckValidator()
    {
        RuleFor(x => x.QuizId).NotEmpty().WithMessage("Quiz Id is required.");
    }
}
