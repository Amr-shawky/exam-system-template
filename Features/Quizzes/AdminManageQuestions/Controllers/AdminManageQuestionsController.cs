using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Requests;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Controllers;

[ApiController]
[Route("api/admin/quizzes/questions")]
public class AdminManageQuestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminManageQuestionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EndpointResponse>> AddQuestion(
        [FromBody] CreateQuestionRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new AdminCreateQuestionCommand(
            request.QuizId,
            request.Text,
            request.Explanation,
            request.OrderIndex,
            request.Options);

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
        }

        return StatusCode(StatusCodes.Status201Created, EndpointResponse.Ok(result.Message, 201));
    }
}
