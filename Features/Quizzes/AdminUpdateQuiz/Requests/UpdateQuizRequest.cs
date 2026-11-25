namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Requests;

public record UpdateQuizRequest(
    string Title,
    string? Instructions,
    int DurationMinutes,
    int PassScore = 60,
    int? MaxAttempts = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null);
