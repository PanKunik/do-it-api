namespace DoIt.Api.Controllers.Tasks;

public sealed record CreateTaskRequest(
    string Title,
    Guid? TaskListId
);