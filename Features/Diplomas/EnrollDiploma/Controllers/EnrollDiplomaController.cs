using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.EnrollDiploma.Controllers;

[ApiController]
[Route("api/diplomas")]
public class EnrollDiplomaController : ControllerBase
{
    private readonly IMediator _mediator;

    public EnrollDiplomaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{id:guid}/enroll")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EndpointResponse>> EnrollInDiploma(
        [FromRoute] Guid id,
        [FromQuery] Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var command = new EnrollDiplomaCommand(id, studentId);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
        }

        return Ok(EndpointResponse.Ok(result.Message, result.StatusCode));
    }
}
