using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Commands;

public record AdminCreateQuizCommand(
    Guid DiplomaId,
    string Title,
    string? Instructions,
    int DurationMinutes,
    int PassScore,
    int? MaxAttempts,
    DateTime? StartDate,
    DateTime? EndDate) : IRequest<RequestResponse>;
