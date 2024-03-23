using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Core.Words.Queries.GetDefinitions;
using R.Systems.Template.Infrastructure.Azure;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace R.Systems.Template.Api.Web.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAd)]
[RequiredScope("User.Access")]
[Route("words")]
public class WordsController : ControllerBase
{
    public WordsController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get word definitions")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        description: "Correct response",
        type: typeof(List<Definition>),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [Route("{word}/definitions")]
    [HttpGet]
    public async Task<IActionResult> GetDefinitions(
        string? word,
        CancellationToken cancellationToken
    )
    {
        GetDefinitionsResult result = await Mediator.Send(
            new GetDefinitionsQuery { Word = word },
            cancellationToken
        );

        return Ok(result.Definitions);
    }
}
