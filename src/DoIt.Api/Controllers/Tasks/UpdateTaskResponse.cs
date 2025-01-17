namespace DoIt.Api.Controllers.Tasks;

public sealed record UpdateTaskResponse(
    Guid Id,
    string Title,
    DateTime CreatedAt,
    bool IsDone,
    bool IsImportant
);