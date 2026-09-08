using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.BrowseDiplomas.DTOs;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Handlers;

public class BrowseDiplomasQueryHandler : IRequestHandler<BrowseDiplomasQuery, RequestResponse<PaginatedResult<DiplomaListItemDto>>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;

    public BrowseDiplomasQueryHandler(IGenericRepository<Diploma> diplomaRepository)
    {
        _diplomaRepository = diplomaRepository;
    }

    public async Task<RequestResponse<PaginatedResult<DiplomaListItemDto>>> Handle(BrowseDiplomasQuery request, CancellationToken cancellationToken)
    {
        var query = _diplomaRepository.GetAll()
            .Where(d => d.Quizzes.Any(q => q.Status == QuizStatus.Published));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(d => new DiplomaListItemDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                TotalQuizzes = d.Quizzes.Count(q => q.Status == QuizStatus.Published),
                CompletedQuizzes = request.StudentId.HasValue
                    ? d.Quizzes.Count(q => q.Attempts.Any(a =>
                        a.StudentId == request.StudentId.Value &&
                        (a.Status == AttemptStatus.Submitted || a.Status == AttemptStatus.TimedOut)))
                    : 0
            })
            .ToListAsync(cancellationToken);

        var pagedResult = new PaginatedResult<DiplomaListItemDto>(items, totalCount, request.PageIndex, request.PageSize);
        return RequestResponse<PaginatedResult<DiplomaListItemDto>>.Ok(pagedResult);
    }
}
