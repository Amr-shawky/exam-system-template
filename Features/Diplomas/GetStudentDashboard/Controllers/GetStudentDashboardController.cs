using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Diplomas.GetStudentDashboard.Queries;
using exam_system.Features.Diplomas.GetStudentDashboard.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Controllers;

[ApiController]
[Route("api/students")]
public class GetStudentDashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public GetStudentDashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me/dashboard")]
    [ProducesResponseType(typeof(EndpointResponse<StudentDashboardViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EndpointResponse<StudentDashboardViewModel>>> GetStudentDashboard(
        [FromQuery] Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetStudentDashboardQuery(studentId);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.Success || result.Data == null)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode));
        }

        var viewModel = new StudentDashboardViewModel
        {
            StudentId = result.Data.StudentId,
            TotalEnrolledDiplomas = result.Data.TotalEnrolledDiplomas,
            TotalAttempts = result.Data.TotalAttempts,
            AverageScore = result.Data.AverageScore,
            PassRate = result.Data.PassRate,
            EnrolledDiplomas = result.Data.EnrolledDiplomas.Select(d => new EnrolledDiplomaSummaryViewModel
            {
                DiplomaId = d.DiplomaId,
                Title = d.Title,
                EnrolledAt = d.EnrolledAt,
                CompletedQuizzes = d.CompletedQuizzes,
                TotalQuizzes = d.TotalQuizzes
            }).ToList(),
            RecentAttempts = result.Data.RecentAttempts.Select(a => new RecentAttemptSummaryViewModel
            {
                AttemptId = a.AttemptId,
                QuizTitle = a.QuizTitle,
                Score = a.Score,
                Passed = a.Passed,
                SubmittedAt = a.SubmittedAt
            }).ToList()
        };

        return Ok(EndpointResponse<StudentDashboardViewModel>.Ok(viewModel));
    }
}
