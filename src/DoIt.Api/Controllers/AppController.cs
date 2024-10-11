using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers;

[Route("api/[controller]")]
public class AppController : ControllerBase
{
    public async Task<IActionResult> HealthCheck()
    {
        await Task.CompletedTask;
        return Ok();
    }
}
