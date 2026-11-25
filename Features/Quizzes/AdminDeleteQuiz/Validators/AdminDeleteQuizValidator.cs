using FluentValidation;
using exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Validators;

public class AdminDeleteQuizValidator : AbstractValidator<AdminDeleteQuizCommand>
{
    public AdminDeleteQuizValidator()
    {
        RuleFor(x => x.QuizId).NotEmpty().WithMessage("Quiz Id is required.");
    }
}
