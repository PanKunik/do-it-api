using DoIt.Api.Services.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers.Tasks;

[ApiController]
[Route("api/tasks")]
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
        var result = await _tasksService.GetById(id);
        
        return result.Map<IActionResult>(
            onSuccess: Ok,
            onFailure: error => NotFound(error)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request)
    {
        var result = await _tasksService.Create(request);

        return result.Map<IActionResult>(
            onSuccess: (createdTask)
                => CreatedAtAction(
                    nameof(GetById),
                    new { id =  createdTask.Id },
                    createdTask
                ),
            onFailure: error => BadRequest(error) // TODO: Return not only BadRequest() responses - ProblemDetails
        );
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _tasksService.Delete(id);

        return result.Map<IActionResult>(
            onSuccess: _ => NoContent(),
            onFailure: error => NotFound(error) // TODO: Return not only NotFound() responses - ProblemDetails
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateTaskRequest request
    )
    {
        var result = await _tasksService.Update(id, request);

        return result.Map<IActionResult>(
            onSuccess: _ => NoContent(),
            onFailure: error => NotFound(error) // TODO: Return not only NotFound() responses - ProblemDetails
        );
    }
}