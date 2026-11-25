using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Controllers;

[ApiController]
[Route("api/admin/quizzes")]
public class AdminDeleteQuizController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminDeleteQuizController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EndpointResponse>> DeleteQuiz(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new AdminDeleteQuizCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
        }

        return Ok(EndpointResponse.Ok(result.Message));
    }
}
