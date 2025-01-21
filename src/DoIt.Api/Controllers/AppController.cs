using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers;

[ApiController]
[Route("api/app")]
public class AppController : ControllerBase
{
    public async Task<IActionResult> HealthCheck()
    {
        await Task.CompletedTask;
        return Ok(200);
    }
}
