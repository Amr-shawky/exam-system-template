using FluentValidation;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Validators;

public class AdminUpdateQuizValidator : AbstractValidator<AdminUpdateQuizCommand>
{
    public AdminUpdateQuizValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Quiz Id is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Quiz title is required.")
            .Length(3, 200).WithMessage("Title must be between 3 and 200 characters.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be a positive integer.");

        RuleFor(x => x.PassScore)
            .InclusiveBetween(0, 100).WithMessage("Pass score must be between 0 and 100.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate!.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("End date must be after start date.");
    }
}
