using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Controllers;

[ApiController]
[Route("api/admin/quizzes")]
public class AdminQuizPublishCheckController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminQuizPublishCheckController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}/publish-check")]
    [ProducesResponseType(typeof(EndpointResponse<PublishChecklistViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EndpointResponse<PublishChecklistViewModel>>> CheckPublishReadiness(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new AdminQuizPublishCheckQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.Success || result.Data == null)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode));
        }

        var viewModel = new PublishChecklistViewModel
        {
            QuizId = result.Data.QuizId,
            QuizTitle = result.Data.QuizTitle,
            IsReadyToPublish = result.Data.IsReadyToPublish,
            HasQuestions = result.Data.HasQuestions,
            QuestionCount = result.Data.QuestionCount,
            AllQuestionsHaveSingleCorrectOption = result.Data.AllQuestionsHaveSingleCorrectOption,
            HasValidDuration = result.Data.HasValidDuration,
            HasValidPassScore = result.Data.HasValidPassScore,
            HasValidDateRange = result.Data.HasValidDateRange,
            ValidationErrors = result.Data.ValidationErrors
        };

        return Ok(EndpointResponse<PublishChecklistViewModel>.Ok(viewModel));
    }
}
