using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers;

public class AdminDeleteDiplomaCommandHandler : IRequestHandler<AdminDeleteDiplomaCommand, RequestResponse>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;

    public AdminDeleteDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepository)
    {
        _diplomaRepository = diplomaRepository;
    }

    public async Task<RequestResponse> Handle(AdminDeleteDiplomaCommand request, CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository.GetByIdAsync(request.Id);
        if (diploma == null)
        {
            return RequestResponse.Fail($"Diploma with Id {request.Id} was not found.", 404);
        }

        var hasActiveEnrollments = await _diplomaRepository.GetAll()
            .Where(d => d.Id == request.Id)
            .Select(d => d.Enrollments.Any())
            .FirstOrDefaultAsync(cancellationToken);

        if (hasActiveEnrollments)
        {
            return RequestResponse.Fail("Cannot delete diploma that has active enrollments.", 409);
        }

        await _diplomaRepository.DeleteAsync(diploma);
        return RequestResponse.Ok("Diploma deleted successfully.");
    }
}
