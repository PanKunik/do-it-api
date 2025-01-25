using DoIt.Api.Controllers.Errors;
using DoIt.Api.Shared;
using DoIt.Api.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DoIt.Api.Controllers._Common;

[ApiController]
public class ApiController
    : ControllerBase
{
    protected IActionResult Problem(Error error)
    {
        HttpContext.Items[Constants.Error.ErrorsName] = error;

        var statusCode = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(
            statusCode: statusCode,
            title: error.Message
        );
    }
}
