using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Controllers;

[ApiController]
[Route("api/admin/quizzes")]
public class AdminUnpublishQuizController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminUnpublishQuizController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPatch("{id:guid}/unpublish")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EndpointResponse>> UnpublishQuiz(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new AdminUnpublishQuizCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
        }

        return Ok(EndpointResponse.Ok(result.Message));
    }
}
