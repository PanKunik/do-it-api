using DoIt.Api.Services.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TasksController(ITasksService tasksService)
    : ControllerBase
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
        var task = await _tasksService.GetById(id);
        return task is not null
            ? Ok(task)
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request)
    {
        var createdTask = await _tasksService.Create(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdTask.Id.ToString("N") },
            createdTask
        );
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var wasDeleted = await _tasksService.Delete(id);
        return wasDeleted
            ? NoContent()
            : NotFound();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateTaskRequest request
    )
    {
        var result = await _tasksService.Update(id, request);
        return result is null
            ? NotFound()
            : NoContent();
    }
}