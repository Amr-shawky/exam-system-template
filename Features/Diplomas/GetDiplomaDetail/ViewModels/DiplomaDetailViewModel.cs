namespace exam_system.Features.Diplomas.GetDiplomaDetail.ViewModels;

public class DiplomaDetailViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<DiplomaQuizItemViewModel> Quizzes { get; set; } = new();
}

public class DiplomaQuizItemViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int PassScore { get; set; }
    public int? MaxAttempts { get; set; }
    public bool CanAttempt { get; set; }
    public bool IsResumable { get; set; }
}
