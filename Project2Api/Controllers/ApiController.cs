using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Project2Api.ServiceErrors;

namespace Project2Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    /// <summary>
    /// Returns Problem with error code and description
    /// </summary>
    /// <param name="errors"></param>
    /// <returns>Problem</returns>
    protected IActionResult Problem(List<Error> errors)
    {
        var firstError = errors[0];
        var StatusCode = (int)firstError.Type switch
        {
            (int)ErrorType.NotFound => StatusCodes.Status404NotFound,
            (int)CustomErrorType.Database => StatusCodes.Status500InternalServerError,
            (int)CustomErrorType.InvalidParams => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError   
        };

        return Problem(
            detail: firstError.Description,
            statusCode: StatusCode,
            title: firstError.Code
        );
    }
}