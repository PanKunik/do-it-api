namespace DoIt.Api.Controllers.Tasks;

public sealed record GetTaskResponse(
    Guid taskId,
    string title,
    DateTime createdAt,
    bool isDone,
    bool isImportant
);