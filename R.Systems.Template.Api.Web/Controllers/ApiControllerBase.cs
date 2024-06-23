using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core.Common.Errors;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.Api.Web.Controllers;

[ApiController]
[SwaggerResponse(
    StatusCodes.Status422UnprocessableEntity,
    "Validation errors",
    typeof(IEnumerable<ErrorInfo>),
    [MediaTypeNames.Application.Json]
)]
[SwaggerResponse(
    StatusCodes.Status500InternalServerError
)]
public class ApiControllerBase : ControllerBase
{
}
