using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Handlers;

public class AdminPublishQuizCommandHandler : IRequestHandler<AdminPublishQuizCommand, RequestResponse>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public AdminPublishQuizCommandHandler(IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<RequestResponse> Handle(AdminPublishQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);
        if (quiz == null)
        {
            return RequestResponse.Fail($"Quiz with Id {request.QuizId} was not found.", 404);
        }

        // Projection check on questions readiness
        var questionStats = await _quizRepository.GetAll()
            .Where(q => q.Id == request.QuizId)
            .Select(q => new
            {
                Total = q.Questions.Count,
                Invalid = q.Questions.Count(qn => qn.Options.Count < 2 || qn.Options.Count(o => o.IsCorrect) != 1)
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (questionStats == null || questionStats.Total == 0)
        {
            return RequestResponse.Fail("Cannot publish quiz with zero questions.", 400);
        }

        if (questionStats.Invalid > 0)
        {
            return RequestResponse.Fail("Cannot publish quiz because some questions lack valid options or correct answers.", 400);
        }

        quiz.Status = QuizStatus.Published;
        quiz.PublishedAt = DateTime.UtcNow;

        await _quizRepository.UpdateAsync(quiz);
        return RequestResponse.Ok("Quiz published successfully.");
    }
}
