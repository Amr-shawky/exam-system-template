using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class AdminCreateQuestionCommandHandler : IRequestHandler<AdminCreateQuestionCommand, RequestResponse>
{
    // Injects ONLY 1 repository: IGenericRepository<Question>
    private readonly IGenericRepository<Question> _questionRepository;

    public AdminCreateQuestionCommandHandler(IGenericRepository<Question> questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<RequestResponse> Handle(AdminCreateQuestionCommand request, CancellationToken cancellationToken)
    {
        // EXAM-15: Questions cannot be added to a quiz if it is already published
        var question = new Question
        {
            QuizId = request.QuizId,
            Text = request.Text.Trim(),
            Explanation = request.Explanation?.Trim(),
            OrderIndex = request.OrderIndex,
            Options = request.Options.Select(o => new QuestionOption
            {
                OptionText = o.OptionText.Trim(),
                IsCorrect = o.IsCorrect
            }).ToList()
        };

        await _questionRepository.AddAsync(question);
        return RequestResponse.Ok("Question and options added successfully.", 201);
    }
}
