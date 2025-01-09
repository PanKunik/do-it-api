namespace DoIt.Api.Controllers.Tasks;

public sealed record CreateTaskResponse(
    Guid Id,
    string Title,
    DateTime CreatedAt,
    bool IsDone,
    bool IsImportant
);
