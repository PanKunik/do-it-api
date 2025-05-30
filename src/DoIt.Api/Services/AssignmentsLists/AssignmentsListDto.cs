using DoIt.Api.Services.Assignments;

namespace DoIt.Api.Services.AssignmentsLists;

public sealed record AssignmentsListDto(
    Guid Id,
    string Name,
    DateTime CreatedAt,
    List<AssignmentDto>? Assignments
);