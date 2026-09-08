using MediatR;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Handlers;

public class AdminCreateDiplomaCommandHandler : IRequestHandler<AdminCreateDiplomaCommand, RequestResponse>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;

    public AdminCreateDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepository)
    {
        _diplomaRepository = diplomaRepository;
    }

    public async Task<RequestResponse> Handle(AdminCreateDiplomaCommand request, CancellationToken cancellationToken)
    {
        var diploma = new Diploma
        {
            Title = request.Title.Trim(),
            Description = request.Description?.Trim()
        };

        await _diplomaRepository.AddAsync(diploma);
        return RequestResponse.Ok("Diploma created successfully.", 201);
    }
}
