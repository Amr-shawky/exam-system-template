using FluentValidation;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Validators;

public class AdminUnpublishQuizValidator : AbstractValidator<AdminUnpublishQuizCommand>
{
    public AdminUnpublishQuizValidator()
    {
        RuleFor(x => x.QuizId).NotEmpty().WithMessage("Quiz Id is required.");
    }
}
