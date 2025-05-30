using DoIt.Api.Domain.AssignmentsLists;
using DoIt.Api.Domain.Assignments;

namespace DoIt.Api.TestUtils;

[Obsolete("Move to builders")]
public static class TaskListsUtilities
{
    public static List<Assignment> CreateTasks(int tasksCount = 1, AssignmentsListId? taskListId = null)
        => Enumerable
            .Range(0, tasksCount)
            .Select(r => Assignment.Create(
                    Constants.Tasks.TaskIdFromIndex(r),
                    Constants.Tasks.TitleFromIndex(r),
                    Constants.Tasks.CreatedAtFromIndex(r),
                    Constants.Tasks.NotDone,
                    Constants.Tasks.NotImportant,
                    taskListId
                ).Value!
            )
            .ToList();
}