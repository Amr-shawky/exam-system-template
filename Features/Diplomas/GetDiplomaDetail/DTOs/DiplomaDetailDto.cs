namespace exam_system.Features.Diplomas.GetDiplomaDetail.DTOs;

public class DiplomaDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<DiplomaQuizItemDto> Quizzes { get; set; } = new();
}

public class DiplomaQuizItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int PassScore { get; set; }
    public int? MaxAttempts { get; set; }
    public bool CanAttempt { get; set; }
    public bool IsResumable { get; set; }
}
