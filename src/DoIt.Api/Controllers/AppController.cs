using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppController : ControllerBase
{
    public async Task<IActionResult> HealthCheck()
    {
        await Task.CompletedTask;
        return Ok(200);
    }
}
