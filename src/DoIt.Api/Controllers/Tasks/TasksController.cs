using DoIt.Api.Persistence.Repositories.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers.Tasks
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(ITasksRepository repository)
        : ControllerBase
    {
        private readonly ITasksRepository _repository = repository;

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> Get()
        {
            var result = await _repository.GetAll();
            // TODO: Mapping DTO (Domain?) -> reposonse
            return Ok(result.Select(t => new GetTaskResponse(t.TaskId, t.Title, t.CreatedAt, t.IsDone, t.IsImportant)).ToList());
        }
    }
}