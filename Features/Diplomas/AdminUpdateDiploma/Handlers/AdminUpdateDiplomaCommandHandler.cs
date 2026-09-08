using MediatR;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Handlers;

public class AdminUpdateDiplomaCommandHandler : IRequestHandler<AdminUpdateDiplomaCommand, RequestResponse>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;

    public AdminUpdateDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepository)
    {
        _diplomaRepository = diplomaRepository;
    }

    public async Task<RequestResponse> Handle(AdminUpdateDiplomaCommand request, CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository.GetByIdAsync(request.Id);
        if (diploma == null)
        {
            return RequestResponse.Fail($"Diploma with Id {request.Id} was not found.", 404);
        }

        diploma.Title = request.Title.Trim();
        diploma.Description = request.Description?.Trim();

        await _diplomaRepository.UpdateAsync(diploma);
        return RequestResponse.Ok("Diploma updated successfully.");
    }
}
