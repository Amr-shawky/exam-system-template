namespace exam_system.Features.Quizzes.AdminManageQuestions.Requests;

public record CreateQuestionOptionRequest(string OptionText, bool IsCorrect);

public record CreateQuestionRequest(
    Guid QuizId,
    string Text,
    string? Explanation,
    int OrderIndex,
    List<CreateQuestionOptionRequest> Options);
