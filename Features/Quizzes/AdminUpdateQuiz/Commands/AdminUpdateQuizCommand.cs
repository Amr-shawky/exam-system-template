using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;

public record AdminUpdateQuizCommand(
    Guid Id,
    string Title,
    string? Instructions,
    int DurationMinutes,
    int PassScore,
    int? MaxAttempts,
    DateTime? StartDate,
    DateTime? EndDate) : IRequest<RequestResponse>;
