using MediatR;
using exam_system.Features.Diplomas.GetDiplomaDetail.DTOs;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Queries;

public record GetDiplomaDetailQuery(Guid Id, Guid? StudentId) 
    : IRequest<RequestResponse<DiplomaDetailDto>>;
