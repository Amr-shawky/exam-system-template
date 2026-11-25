using MediatR;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.DTOs;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;

public record AdminQuizPublishCheckQuery(Guid QuizId) : IRequest<RequestResponse<PublishChecklistDto>>;
