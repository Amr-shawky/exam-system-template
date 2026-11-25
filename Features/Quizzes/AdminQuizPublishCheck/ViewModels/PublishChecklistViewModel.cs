namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.ViewModels;

public class PublishChecklistViewModel
{
    public Guid QuizId { get; set; }
    public string QuizTitle { get; set; } = string.Empty;
    public bool IsReadyToPublish { get; set; }
    public bool HasQuestions { get; set; }
    public int QuestionCount { get; set; }
    public bool AllQuestionsHaveSingleCorrectOption { get; set; }
    public bool HasValidDuration { get; set; }
    public bool HasValidPassScore { get; set; }
    public bool HasValidDateRange { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
}
