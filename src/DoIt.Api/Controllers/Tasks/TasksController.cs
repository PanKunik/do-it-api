using DoIt.Api.Controllers._Common;
using DoIt.Api.Services.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers.Tasks;

[Route("api/tasks")]
public class TasksController(ITasksService tasksService)
    : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var tasks = await tasksService.GetAll();
        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await tasksService.GetById(id);
        
        return result.Map(
            onSuccess: Ok,
            onFailure: Problem
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request)
    {
        var result = await tasksService.Create(request);

        return result.Map(
            onSuccess: (createdTask)
                => CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { id =  createdTask.Id },
                    value: null
                ),
            onFailure: Problem
        );
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await tasksService.Delete(id);

        return result.Map(
            onSuccess: NoContent,
            onFailure: Problem
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateTaskRequest request
    )
    {
        var result = await tasksService.Update(id, request);

        return result.Map(
            onSuccess: NoContent,
            onFailure: Problem
        );
    }

    [HttpPut("{id:guid}/change-state")]
    public async Task<IActionResult> ChangeState(Guid id)
    {
        var result = await tasksService.ChangeState(id);

        return result.Map(
            onSuccess: NoContent,
            onFailure: Problem
        );
    }

    [HttpPut("{id:guid}/change-importance")]
    public async Task<IActionResult> ChangeImportance(Guid id)
    {
        var result = await tasksService.ChangeImportance(id);

        return result.Map(
            onSuccess: NoContent,
            onFailure: Problem
        );
    }
}