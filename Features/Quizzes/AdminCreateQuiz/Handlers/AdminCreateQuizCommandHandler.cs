using MediatR;
using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminCreateQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Handlers;

public class AdminCreateQuizCommandHandler : IRequestHandler<AdminCreateQuizCommand, RequestResponse>
{
    // Injects ONLY 1 repository: IGenericRepository<Quiz>
    private readonly IGenericRepository<Quiz> _quizRepository;

    public AdminCreateQuizCommandHandler(IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<RequestResponse> Handle(AdminCreateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = new Quiz
        {
            DiplomaId = request.DiplomaId,
            Title = request.Title.Trim(),
            Instructions = request.Instructions?.Trim(),
            DurationMinutes = request.DurationMinutes,
            PassScore = request.PassScore,
            MaxAttempts = request.MaxAttempts,
            Status = QuizStatus.Draft,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        await _quizRepository.AddAsync(quiz);
        return RequestResponse.Ok("Quiz created successfully as draft.", 201);
    }
}
