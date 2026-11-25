using MediatR;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Handlers;

public class AdminUpdateQuizCommandHandler : IRequestHandler<AdminUpdateQuizCommand, RequestResponse>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public AdminUpdateQuizCommandHandler(IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<RequestResponse> Handle(AdminUpdateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.Id);
        if (quiz == null)
        {
            return RequestResponse.Fail($"Quiz with Id {request.Id} was not found.", 404);
        }

        quiz.Title = request.Title.Trim();
        quiz.Instructions = request.Instructions?.Trim();
        quiz.DurationMinutes = request.DurationMinutes;
        quiz.PassScore = request.PassScore;
        quiz.MaxAttempts = request.MaxAttempts;
        quiz.StartDate = request.StartDate;
        quiz.EndDate = request.EndDate;

        await _quizRepository.UpdateAsync(quiz);
        return RequestResponse.Ok("Quiz updated successfully.");
    }
}
