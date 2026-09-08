using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Common.Enums;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Diplomas.GetStudentDashboard.DTOs;
using exam_system.Features.Diplomas.GetStudentDashboard.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Handlers;

public class GetStudentDashboardQueryHandler : IRequestHandler<GetStudentDashboardQuery, RequestResponse<StudentDashboardDto>>
{
    private readonly IGenericRepository<Student> _studentRepository;

    public GetStudentDashboardQueryHandler(IGenericRepository<Student> studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<RequestResponse<StudentDashboardDto>> Handle(GetStudentDashboardQuery request, CancellationToken cancellationToken)
    {
        var studentData = await _studentRepository.GetAll()
            .Where(s => s.Id == request.StudentId)
            .Select(s => new
            {
                s.Id,
                EnrolledDiplomas = s.Enrollments.Select(e => new EnrolledDiplomaSummaryDto
                {
                    DiplomaId = e.DiplomaId,
                    Title = e.Diploma.Title,
                    EnrolledAt = e.EnrolledAt,
                    TotalQuizzes = e.Diploma.Quizzes.Count(q => q.Status == QuizStatus.Published),
                    CompletedQuizzes = e.Diploma.Quizzes.Count(q =>
                        q.Attempts.Any(a => a.StudentId == s.Id &&
                            (a.Status == AttemptStatus.Submitted || a.Status == AttemptStatus.TimedOut)))
                }).ToList(),
                Attempts = s.QuizAttempts
                    .Where(a => a.Status == AttemptStatus.Submitted || a.Status == AttemptStatus.TimedOut)
                    .OrderByDescending(a => a.SubmittedAt)
                    .Select(a => new RecentAttemptSummaryDto
                    {
                        AttemptId = a.Id,
                        QuizTitle = a.Quiz.Title,
                        Score = a.Score,
                        Passed = a.Passed,
                        SubmittedAt = a.SubmittedAt
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (studentData == null)
        {
            return RequestResponse<StudentDashboardDto>.Fail("Student not found.", 404);
        }

        var attempts = studentData.Attempts;
        var totalAttempts = attempts.Count;
        var avgScore = totalAttempts > 0 ? attempts.Average(a => a.Score ?? 0) : 0;
        var passRate = totalAttempts > 0 ? (attempts.Count(a => a.Passed == true) / (double)totalAttempts) * 100 : 0;

        var dto = new StudentDashboardDto
        {
            StudentId = studentData.Id,
            TotalEnrolledDiplomas = studentData.EnrolledDiplomas.Count,
            TotalAttempts = totalAttempts,
            AverageScore = Math.Round(avgScore, 2),
            PassRate = Math.Round(passRate, 2),
            EnrolledDiplomas = studentData.EnrolledDiplomas,
            RecentAttempts = attempts.Take(5).ToList()
        };

        return RequestResponse<StudentDashboardDto>.Ok(dto);
    }
}
