namespace DoIt.Api.Controllers.Tasks;

public sealed record GetTaskResponse(
    Guid Id,
    string Title,
    DateTime CreatedAt,
    bool IsDone,
    bool IsImportant
);