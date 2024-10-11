using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers;

public class AppController : ControllerBase
{
    public async Task<IActionResult> HealthCheck()
    {
        await Task.CompletedTask;
        return Ok();
    }
}
