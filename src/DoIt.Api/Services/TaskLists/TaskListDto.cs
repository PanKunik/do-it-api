using DoIt.Api.Services.Tasks;

namespace DoIt.Api.Services.TaskLists;

public sealed record TaskListDto(
    Guid Id,
    string Name,
    DateTime CreatedAt,
    List<TaskDto> Tasks
);