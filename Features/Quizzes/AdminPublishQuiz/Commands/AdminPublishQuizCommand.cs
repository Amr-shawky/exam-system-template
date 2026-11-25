using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Commands;

public record AdminPublishQuizCommand(Guid QuizId) : IRequest<RequestResponse>;
