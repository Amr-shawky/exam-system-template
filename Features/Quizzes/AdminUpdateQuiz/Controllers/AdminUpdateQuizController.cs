using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Requests;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Controllers;

[ApiController]
[Route("api/admin/quizzes")]
public class AdminUpdateQuizController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminUpdateQuizController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EndpointResponse>> UpdateQuiz(
        [FromRoute] Guid id,
        [FromBody] UpdateQuizRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new AdminUpdateQuizCommand(
            id,
            request.Title,
            request.Instructions,
            request.DurationMinutes,
            request.PassScore,
            request.MaxAttempts,
            request.StartDate,
            request.EndDate);

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
        }

        return Ok(EndpointResponse.Ok(result.Message));
    }
}
