using DoIt.Api.Controllers._Common;
using DoIt.Api.Services.TaskLists;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers.TaskLists;

[Route("api/task-lists")]
public class TaskListsController(ITaskListsService taskListsService)
    : ApiController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskListRequest request)
    {
        var result = await taskListsService.Create(request);

        return result.Map(
            onSuccess: (createdTaskList)
                => CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { id = createdTaskList.Id },
                    value: null
                ),
                onFailure: Problem
        );
    }

    public async Task<IActionResult> GetById(int id)
    {
        return null;
    }
}