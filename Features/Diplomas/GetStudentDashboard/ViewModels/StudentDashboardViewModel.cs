namespace exam_system.Features.Diplomas.GetStudentDashboard.ViewModels;

public class StudentDashboardViewModel
{
    public Guid StudentId { get; set; }
    public int TotalEnrolledDiplomas { get; set; }
    public int TotalAttempts { get; set; }
    public double AverageScore { get; set; }
    public double PassRate { get; set; }
    public List<EnrolledDiplomaSummaryViewModel> EnrolledDiplomas { get; set; } = new();
    public List<RecentAttemptSummaryViewModel> RecentAttempts { get; set; } = new();
}

public class EnrolledDiplomaSummaryViewModel
{
    public Guid DiplomaId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
    public int CompletedQuizzes { get; set; }
    public int TotalQuizzes { get; set; }
}

public class RecentAttemptSummaryViewModel
{
    public Guid AttemptId { get; set; }
    public string QuizTitle { get; set; } = string.Empty;
    public double? Score { get; set; }
    public bool? Passed { get; set; }
    public DateTime? SubmittedAt { get; set; }
}
