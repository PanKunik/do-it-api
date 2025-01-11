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
        var result = await _tasksService.GetAll();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _tasksService.GetById(id);
        return result is not null
            ? Ok(result)
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateTaskRequest request
    )
    {
        var result = await _tasksService.Create(request);

        return CreatedAtAction(
            nameof(Get),
            result.Id,
            result
        );
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _tasksService.Delete(id);
        return NoContent();
    }
}