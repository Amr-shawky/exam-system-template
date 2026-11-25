namespace exam_system.Features.Quizzes.AdminCreateQuiz.Requests;

public record CreateQuizRequest(
    Guid DiplomaId,
    string Title,
    string? Instructions,
    int DurationMinutes,
    int PassScore = 60,
    int? MaxAttempts = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null);
