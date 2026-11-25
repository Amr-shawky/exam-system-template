using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Quizzes.AdminCreateQuiz.Commands;
using exam_system.Features.Quizzes.AdminCreateQuiz.Requests;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Controllers;

[ApiController]
[Route("api/admin/quizzes")]
public class AdminCreateQuizController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminCreateQuizController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EndpointResponse>> CreateQuiz(
        [FromBody] CreateQuizRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new AdminCreateQuizCommand(
            request.DiplomaId,
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

        return StatusCode(StatusCodes.Status201Created, EndpointResponse.Ok(result.Message, 201));
    }
}
