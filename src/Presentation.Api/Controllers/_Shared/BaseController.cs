using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace Presentation.Api.Controllers._Shared;

[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
[SwaggerResponse(200, Type = typeof(IEnumerable<string>))]
[SwaggerResponse(400, Type = typeof(IEnumerable<string>))]
[SwaggerResponse(401, Type = typeof(IEnumerable<string>))]
[SwaggerResponse(403, Type = typeof(IEnumerable<string>))]
public class BaseController : ControllerBase
{
    protected IActionResult HandlerResponse(HttpStatusCode statusCode, object result)
        => StatusCode((int)statusCode, result);
}