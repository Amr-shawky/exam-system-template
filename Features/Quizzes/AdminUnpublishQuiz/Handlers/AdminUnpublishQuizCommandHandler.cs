using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Handlers;

public class AdminUnpublishQuizCommandHandler : IRequestHandler<AdminUnpublishQuizCommand, RequestResponse>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public AdminUnpublishQuizCommandHandler(IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<RequestResponse> Handle(AdminUnpublishQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);
        if (quiz == null)
        {
            return RequestResponse.Fail($"Quiz with Id {request.QuizId} was not found.", 404);
        }

        // EXAM-17: Blocked with 409 if any student currently has an in-progress attempt
        var hasInProgressAttempts = await _quizRepository.GetAll()
            .Where(q => q.Id == request.QuizId)
            .Select(q => q.Attempts.Any(a => a.Status == AttemptStatus.InProgress))
            .FirstOrDefaultAsync(cancellationToken);

        if (hasInProgressAttempts)
        {
            return RequestResponse.Fail("Cannot unpublish quiz while students have attempts in progress.", 409);
        }

        quiz.Status = QuizStatus.Draft;
        await _quizRepository.UpdateAsync(quiz);
        return RequestResponse.Ok("Quiz unpublished successfully.");
    }
}
