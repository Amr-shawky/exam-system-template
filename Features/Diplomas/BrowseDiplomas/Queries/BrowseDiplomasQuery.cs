using MediatR;
using exam_system.Features.Diplomas.BrowseDiplomas.DTOs;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Queries;

public record BrowseDiplomasQuery(Guid? StudentId, int PageIndex = 1, int PageSize = 10) 
    : IRequest<RequestResponse<PaginatedResult<DiplomaListItemDto>>>;
