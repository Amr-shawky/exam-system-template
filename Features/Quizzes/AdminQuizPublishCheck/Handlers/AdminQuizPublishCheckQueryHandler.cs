using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.DTOs;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Handlers;

public class AdminQuizPublishCheckQueryHandler : IRequestHandler<AdminQuizPublishCheckQuery, RequestResponse<PublishChecklistDto>>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public AdminQuizPublishCheckQueryHandler(IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<RequestResponse<PublishChecklistDto>> Handle(AdminQuizPublishCheckQuery request, CancellationToken cancellationToken)
    {
        // EXAM-16: Read-only readiness checklist without mutating data
        var quizData = await _quizRepository.GetAll()
            .Where(q => q.Id == request.QuizId)
            .Select(q => new
            {
                q.Id,
                q.Title,
                q.DurationMinutes,
                q.PassScore,
                q.StartDate,
                q.EndDate,
                QuestionCount = q.Questions.Count,
                InvalidQuestionsCount = q.Questions.Count(qn =>
                    qn.Options.Count < 2 || qn.Options.Count(o => o.IsCorrect) != 1)
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (quizData == null)
        {
            return RequestResponse<PublishChecklistDto>.Fail($"Quiz with Id {request.QuizId} was not found.", 404);
        }

        var errors = new List<string>();

        var hasQuestions = quizData.QuestionCount > 0;
        if (!hasQuestions) errors.Add("Quiz must have at least one question.");

        var allCorrectValid = quizData.InvalidQuestionsCount == 0;
        if (!allCorrectValid) errors.Add("Every question must have at least 2 options and exactly one correct option marked.");

        var validDuration = quizData.DurationMinutes > 0;
        if (!validDuration) errors.Add("Duration must be a positive number of minutes.");

        var validPassScore = quizData.PassScore >= 0 && quizData.PassScore <= 100;
        if (!validPassScore) errors.Add("Pass score must be between 0 and 100.");

        var validDateRange = !quizData.StartDate.HasValue || !quizData.EndDate.HasValue || quizData.EndDate > quizData.StartDate;
        if (!validDateRange) errors.Add("End date must be after start date.");

        var isReady = hasQuestions && allCorrectValid && validDuration && validPassScore && validDateRange;

        var dto = new PublishChecklistDto
        {
            QuizId = quizData.Id,
            QuizTitle = quizData.Title,
            IsReadyToPublish = isReady,
            HasQuestions = hasQuestions,
            QuestionCount = quizData.QuestionCount,
            AllQuestionsHaveSingleCorrectOption = allCorrectValid,
            HasValidDuration = validDuration,
            HasValidPassScore = validPassScore,
            HasValidDateRange = validDateRange,
            ValidationErrors = errors
        };

        return RequestResponse<PublishChecklistDto>.Ok(dto);
    }
}
