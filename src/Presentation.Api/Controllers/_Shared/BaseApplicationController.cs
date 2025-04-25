using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Presentation.Api.Controllers._Shared;

[ApiController]
[Route("api/app/v{version:apiVersion}/[controller]")]
[ApiExplorerSettings(GroupName = "Application")]
[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Response))]
[ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(Response))]
[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(Response))]
[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(Response))]
public class BaseApplicationController : BaseController { }