namespace DoIt.Api.Services.Assignments;

public sealed record AssignmentDto(
    Guid Id,
    string Title,
    DateTime CreatedAt,
    bool IsDone,
    bool IsImportant,
    Guid? AssignmentsListId
);