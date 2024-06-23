using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Template.Core.Words.Queries.GetDefinitions;
using R.Systems.Template.Infrastructure.Azure;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.Api.Web.Controllers;

[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAd)]
[RequiredScope("User.Access")]
[Route("words")]
public class WordsController : ApiControllerBase
{
    private readonly ISender _mediator;

    public WordsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "Get word definitions")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Correct response",
        typeof(List<Definition>),
        [MediaTypeNames.Application.Json]
    )]
    [HttpGet("{word}/definitions")]
    public async Task<IActionResult> GetDefinitions(string? word, CancellationToken cancellationToken)
    {
        GetDefinitionsResult result = await _mediator.Send(new GetDefinitionsQuery { Word = word }, cancellationToken);

        return Ok(result.Definitions);
    }
}
