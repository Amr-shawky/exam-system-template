using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;

public record AdminUpdateDiplomaCommand(Guid Id, string Title, string? Description) : IRequest<RequestResponse>;
