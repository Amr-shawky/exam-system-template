using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;
using exam_system.Features.Diplomas.GetDiplomaDetail.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Controllers;

[ApiController]
[Route("api/diplomas")]
public class GetDiplomaDetailController : ControllerBase
{
    private readonly IMediator _mediator;

    public GetDiplomaDetailController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EndpointResponse<DiplomaDetailViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EndpointResponse<DiplomaDetailViewModel>>> GetDiplomaDetail(
        [FromRoute] Guid id,
        [FromQuery] Guid? studentId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetDiplomaDetailQuery(id, studentId);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.Success || result.Data == null)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode));
        }

        var viewModel = new DiplomaDetailViewModel
        {
            Id = result.Data.Id,
            Title = result.Data.Title,
            Description = result.Data.Description,
            Quizzes = result.Data.Quizzes.Select(q => new DiplomaQuizItemViewModel
            {
                Id = q.Id,
                Title = q.Title,
                DurationMinutes = q.DurationMinutes,
                PassScore = q.PassScore,
                MaxAttempts = q.MaxAttempts,
                CanAttempt = q.CanAttempt,
                IsResumable = q.IsResumable
            }).ToList()
        };

        return Ok(EndpointResponse<DiplomaDetailViewModel>.Ok(viewModel));
    }
}
