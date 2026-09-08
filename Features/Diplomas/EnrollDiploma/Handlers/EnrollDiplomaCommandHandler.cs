using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers;

public class EnrollDiplomaCommandHandler : IRequestHandler<EnrollDiplomaCommand, RequestResponse>
{
    private readonly IGenericRepository<StudentEnrollment> _enrollmentRepository;

    public EnrollDiplomaCommandHandler(IGenericRepository<StudentEnrollment> enrollmentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<RequestResponse> Handle(EnrollDiplomaCommand request, CancellationToken cancellationToken)
    {
        var alreadyEnrolled = await _enrollmentRepository.GetAll()
            .AnyAsync(e => e.StudentId == request.StudentId && e.DiplomaId == request.DiplomaId, cancellationToken);

        if (alreadyEnrolled)
        {
            return RequestResponse.Fail("Student is already enrolled in this diploma.", 409);
        }

        var enrollment = new StudentEnrollment
        {
            StudentId = request.StudentId,
            DiplomaId = request.DiplomaId,
            EnrolledAt = DateTime.UtcNow
        };

        await _enrollmentRepository.AddAsync(enrollment);
        return RequestResponse.Ok("Successfully enrolled in diploma.", 200);
    }
}
