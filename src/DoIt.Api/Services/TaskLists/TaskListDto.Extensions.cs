using DoIt.Api.Domain.TaskLists;

namespace DoIt.Api.Services.TaskLists;

public static class Extensions
{
    public static TaskListDto ToDto(this TaskList taskList)
        => new(
                taskList.Id.Value,
                taskList.Name.Value,
                taskList.CreatedAt
            );
}