using DoIt.Api.Domain.TaskLists;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.TestUtils;

public static class TaskListsUtilities
{
    public static TaskList CreateTaskList(List<Task>? tasks = null)
        => TaskList.Create(
                Constants.TaskLists.TaskListId,
                Constants.TaskLists.Name,
                Constants.TaskLists.CreatedAt,
                tasks ?? CreateTasks(3)
            ).Value!;

    public static List<Task> CreateTasks(int tasksCount = 1)
        => Enumerable
            .Range(0, tasksCount)
            .Select(r => Task.Create(
                    Constants.Tasks.TaskIdFromIndex(r),
                    Constants.Tasks.TitleFromIndex(r),
                    Constants.Tasks.CreatedAtFromIndex(r),
                    Constants.Tasks.NotDone,
                    Constants.Tasks.NotImportant,
                    Constants.TaskLists.TaskListId
                ).Value!
            )
            .ToList();
}