using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;

public record AdminDeleteQuizCommand(Guid QuizId) : IRequest<RequestResponse>;
