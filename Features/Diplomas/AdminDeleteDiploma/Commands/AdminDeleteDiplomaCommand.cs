using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;

public record AdminDeleteDiplomaCommand(Guid Id) : IRequest<RequestResponse>;
