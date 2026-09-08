using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Requests;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Controllers;

[ApiController]
[Route("api/admin/diplomas")]
public class AdminUpdateDiplomaController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminUpdateDiplomaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EndpointResponse>> UpdateDiploma(
        [FromRoute] Guid id,
        [FromBody] UpdateDiplomaRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new AdminUpdateDiplomaCommand(id, request.Title, request.Description);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
        }

        return Ok(EndpointResponse.Ok(result.Message));
    }
}
