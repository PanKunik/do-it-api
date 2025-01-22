using DoIt.Api.Controllers._Common;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers;

[Route("api/app")]
public class AppController
    : ApiController
{
    public async Task<IActionResult> HealthCheck()
    {
        await Task.CompletedTask;
        return Ok(200);
    }
}
