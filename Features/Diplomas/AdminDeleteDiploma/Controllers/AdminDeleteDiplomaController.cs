using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Controllers;

[ApiController]
[Route("api/admin/diplomas")]
public class AdminDeleteDiplomaController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminDeleteDiplomaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EndpointResponse>> DeleteDiploma(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new AdminDeleteDiplomaCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
        }

        return Ok(EndpointResponse.Ok(result.Message));
    }
}
