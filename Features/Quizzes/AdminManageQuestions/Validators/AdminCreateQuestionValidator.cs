using FluentValidation;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Validators;

public class AdminCreateQuestionValidator : AbstractValidator<AdminCreateQuestionCommand>
{
    public AdminCreateQuestionValidator()
    {
        RuleFor(x => x.QuizId).NotEmpty().WithMessage("Quiz Id is required.");
        RuleFor(x => x.Text).NotEmpty().WithMessage("Question text is required.");

        RuleFor(x => x.Options)
            .NotNull()
            .Must(options => options != null && options.Count >= 2)
            .WithMessage("A question must have at least 2 options.");

        RuleFor(x => x.Options)
            .Must(options => options != null && options.Count(o => o.IsCorrect) == 1)
            .When(x => x.Options != null && x.Options.Count >= 2)
            .WithMessage("Exactly one option must be marked as correct.");
    }
}
