using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Diplomas.AdminCreateDiploma.Requests;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Controllers;

[ApiController]
[Route("api/admin/diplomas")]
public class AdminCreateDiplomaController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminCreateDiplomaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EndpointResponse>> CreateDiploma(
        [FromBody] CreateDiplomaRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new AdminCreateDiplomaCommand(request.Title, request.Description);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
        }

        return StatusCode(StatusCodes.Status201Created, EndpointResponse.Ok(result.Message, 201));
    }
}
