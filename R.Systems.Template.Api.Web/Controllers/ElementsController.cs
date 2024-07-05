using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Api.Web.Mappers;
using R.Systems.Template.Api.Web.Models;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Elements.Queries.GetElements;
using R.Systems.Template.Infrastructure.Azure;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.Api.Web.Controllers;

[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAd)]
[RequiredScope("User.Access")]
[Route("elements")]
public class ElementsController : ApiControllerBase
{
    private readonly ISender _mediator;

    public ElementsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "Get elements")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Correct response",
        typeof(ListInfo<Element>),
        [MediaTypeNames.Application.Json]
    )]
    [HttpGet]
    public async Task<IActionResult> GetElements(
        [FromQuery] ListRequest listRequest,
        CancellationToken cancellationToken
    )
    {
        ListMapper mapper = new();
        ListParametersDto listParametersDto = mapper.ToListParametersDto(listRequest);
        GetElementsResult result = await _mediator.Send(
            new GetElementsQuery { ListParametersDto = listParametersDto },
            cancellationToken
        );

        return Ok(result.Elements);
    }
}
