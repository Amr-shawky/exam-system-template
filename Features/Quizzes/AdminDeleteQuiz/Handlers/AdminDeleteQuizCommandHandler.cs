using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Handlers;

public class AdminDeleteQuizCommandHandler : IRequestHandler<AdminDeleteQuizCommand, RequestResponse>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public AdminDeleteQuizCommandHandler(IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<RequestResponse> Handle(AdminDeleteQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);
        if (quiz == null)
        {
            return RequestResponse.Fail($"Quiz with Id {request.QuizId} was not found.", 404);
        }

        // EXAM-18: Cannot delete a published quiz; must be unpublished first
        if (quiz.Status == QuizStatus.Published)
        {
            return RequestResponse.Fail("Cannot delete a published quiz. Please unpublish the quiz first.", 409);
        }

        // Soft delete (keeps historical attempts preserved)
        await _quizRepository.DeleteAsync(quiz);
        return RequestResponse.Ok("Quiz deleted successfully.");
    }
}
