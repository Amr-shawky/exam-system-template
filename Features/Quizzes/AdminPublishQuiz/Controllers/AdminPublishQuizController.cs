using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Controllers;

[ApiController]
[Route("api/admin/quizzes")]
public class AdminPublishQuizController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminPublishQuizController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPatch("{id:guid}/publish")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EndpointResponse>> PublishQuiz(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new AdminPublishQuizCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
        }

        return Ok(EndpointResponse.Ok(result.Message));
    }
}
