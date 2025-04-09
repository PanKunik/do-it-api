using DoIt.Api.Domain.TaskLists;
using DoIt.Api.Services.Tasks;

namespace DoIt.Api.Services.TaskLists;

public static class Extensions
{
    public static TaskListDto ToDto(this TaskList taskList)
        => new(
                taskList.Id.Value,
                taskList.Name.Value,
                taskList.CreatedAt,
                taskList.Tasks.Select(t => t.ToDto()).ToList()
            );
}