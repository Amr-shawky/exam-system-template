using FluentValidation;
using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Validators;

public class AdminPublishQuizValidator : AbstractValidator<AdminPublishQuizCommand>
{
    public AdminPublishQuizValidator()
    {
        RuleFor(x => x.QuizId).NotEmpty().WithMessage("Quiz Id is required.");
    }
}
