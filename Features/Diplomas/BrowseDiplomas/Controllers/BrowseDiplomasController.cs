using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Diplomas.BrowseDiplomas.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Controllers;

[ApiController]
[Route("api/diplomas")]
public class BrowseDiplomasController : ControllerBase
{
    private readonly IMediator _mediator;

    public BrowseDiplomasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(EndpointResponse<PaginatedResult<DiplomaListItemViewModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EndpointResponse<PaginatedResult<DiplomaListItemViewModel>>>> BrowseDiplomas(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? studentId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new BrowseDiplomasQuery(studentId, pageIndex, pageSize);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.Success || result.Data == null)
        {
            return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
        }

        var viewModels = result.Data.Items.Select(item => new DiplomaListItemViewModel
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            TotalQuizzes = item.TotalQuizzes,
            CompletedQuizzes = item.CompletedQuizzes
        }).ToList();

        var paginatedViewModel = new PaginatedResult<DiplomaListItemViewModel>(
            viewModels, result.Data.TotalCount, result.Data.PageIndex, result.Data.PageSize);

        return Ok(EndpointResponse<PaginatedResult<DiplomaListItemViewModel>>.Ok(paginatedViewModel));
    }
}
