using DoIt.Api.Controllers._Common;
using DoIt.Api.Services.TaskLists;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers.TaskLists;

[Route("api/task-lists")]
public class TaskListsController(ITaskListsService taskListsService)
    : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var taskLists = await taskListsService.GetAll();
        return Ok(taskLists);
    }
    
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
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await taskListsService.GetById(id);
        
        return result.Map(
            onSuccess: Ok,
            onFailure: Problem
        );
    }
}