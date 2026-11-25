using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;

public record AdminUnpublishQuizCommand(Guid QuizId) : IRequest<RequestResponse>;
