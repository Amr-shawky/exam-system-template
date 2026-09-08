using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Commands;

public record AdminCreateDiplomaCommand(string Title, string? Description) : IRequest<RequestResponse>;
