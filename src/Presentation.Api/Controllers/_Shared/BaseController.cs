using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;
using Application.Model;

namespace Presentation.Api.Controllers._Shared;

[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
[SwaggerResponse(200, Type = typeof(Response))]
[SwaggerResponse(400, Type = typeof(Response))]
[SwaggerResponse(401, Type = typeof(Response))]
[SwaggerResponse(403, Type = typeof(Response))]
public class BaseController : ControllerBase
{
    protected IActionResult HandlerResponse<T>(HttpStatusCode statusCode, Result<T> result) => result.IsSuccess
        ? StatusCode((int)statusCode, new Response(result.Data))
        : ErrorResponse(HttpStatusCode.BadRequest, result.Errors);

    protected IActionResult ErrorResponse(HttpStatusCode statusCode, IEnumerable<string> errors)
        => StatusCode((int)statusCode, _Shared.Response.Failure(errors));
}