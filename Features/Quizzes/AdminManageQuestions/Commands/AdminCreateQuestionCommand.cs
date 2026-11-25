using MediatR;
using exam_system.Features.Quizzes.AdminManageQuestions.Requests;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands;

public record AdminCreateQuestionCommand(
    Guid QuizId,
    string Text,
    string? Explanation,
    int OrderIndex,
    List<CreateQuestionOptionRequest> Options) : IRequest<RequestResponse>;
