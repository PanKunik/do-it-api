using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers.Errors;

[Route("/error")]
public class ErrorsController
    : ControllerBase
{
    public IActionResult Error()
    {
        // TODO: Check why this isn't used anywhere
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Problem();
    }
}