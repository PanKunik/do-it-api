namespace DoIt.Api.Controllers.Tasks;

public sealed record CreateTaskRequest(
    string Title,
    bool? IsImportant,
    Guid? TaskListId
);