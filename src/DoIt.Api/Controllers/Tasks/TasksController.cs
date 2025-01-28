using DoIt.Api.Controllers._Common;
using DoIt.Api.Services.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers.Tasks;

[Route("api/tasks")]
public class TasksController(ITasksService tasksService)
    : ApiController
{
    private readonly ITasksService _tasksService = tasksService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var tasks = await _tasksService.GetAll();
        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _tasksService.GetById(id);
        
        return result.Map(
            onSuccess: Ok,
            onFailure: Problem
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request)
    {
        var result = await _tasksService.Create(request);

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
        var result = await _tasksService.Delete(id);

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
        var result = await _tasksService.Update(id, request);

        return result.Map(
            onSuccess: NoContent,
            onFailure: Problem
        );
    }
}