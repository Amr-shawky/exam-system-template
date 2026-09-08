using MediatR;
using exam_system.Features.Diplomas.GetStudentDashboard.DTOs;
using exam_system.Features.Shared;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Queries;

public record GetStudentDashboardQuery(Guid StudentId) : IRequest<RequestResponse<StudentDashboardDto>>;
