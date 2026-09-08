using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.GetDiplomaDetail.DTOs;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Handlers;

public class GetDiplomaDetailQueryHandler : IRequestHandler<GetDiplomaDetailQuery, RequestResponse<DiplomaDetailDto>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;

    public GetDiplomaDetailQueryHandler(IGenericRepository<Diploma> diplomaRepository)
    {
        _diplomaRepository = diplomaRepository;
    }

    public async Task<RequestResponse<DiplomaDetailDto>> Handle(GetDiplomaDetailQuery request, CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository.GetAll()
            .Where(d => d.Id == request.Id && d.Quizzes.Any(q => q.Status == QuizStatus.Published))
            .Select(d => new DiplomaDetailDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                Quizzes = d.Quizzes
                    .Where(q => q.Status == QuizStatus.Published)
                    .Select(q => new DiplomaQuizItemDto
                    {
                        Id = q.Id,
                        Title = q.Title,
                        DurationMinutes = q.DurationMinutes,
                        PassScore = q.PassScore,
                        MaxAttempts = q.MaxAttempts,
                        IsResumable = request.StudentId.HasValue &&
                            q.Attempts.Any(a => a.StudentId == request.StudentId.Value && a.Status == AttemptStatus.InProgress),
                        CanAttempt = !request.StudentId.HasValue ||
                            !q.MaxAttempts.HasValue ||
                            q.Attempts.Count(a => a.StudentId == request.StudentId.Value &&
                                (a.Status == AttemptStatus.Submitted || a.Status == AttemptStatus.TimedOut)) < q.MaxAttempts.Value
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (diploma == null)
        {
            return RequestResponse<DiplomaDetailDto>.Fail("Diploma not found or has no published quizzes.", 404);
        }

        return RequestResponse<DiplomaDetailDto>.Ok(diploma);
    }
}
