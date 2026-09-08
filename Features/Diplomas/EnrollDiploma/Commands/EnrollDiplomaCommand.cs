using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.EnrollDiploma.Commands;

public record EnrollDiplomaCommand(Guid DiplomaId, Guid StudentId) : IRequest<RequestResponse>;
