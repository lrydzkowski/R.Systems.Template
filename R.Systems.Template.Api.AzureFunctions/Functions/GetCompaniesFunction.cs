using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Services;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;

namespace R.Systems.Template.Api.AzureFunctions.Functions;

internal class GetCompaniesFunction : FunctionBase<GetCompaniesFunction>
{
    public GetCompaniesFunction(
        ILogger<GetCompaniesFunction> logger,
        HttpResponseBuilder httpResponseBuilder,
        ISender mediator,
        IMapper mapper
    ) : base(logger, httpResponseBuilder, mediator, mapper)
    {
    }

    [OpenApiOperation(
        operationId: "GetCompanies",
        Summary = "GetCompanies",
        Description = "It returns the list of companies.",
        Visibility = OpenApiVisibilityType.Important
    )]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(ListInfo<Company>),
        Description = "The list of companies"
    )]
    [Function(nameof(GetCompanies))]
    public async Task<HttpResponseData> GetCompanies(
        [HttpTrigger(AuthorizationLevel.Function, "get")]
        HttpRequestData request
    )
    {
        Logger.LogInformation("C# Start processing GetCompanies function.");

        GetCompaniesResult result =
            await Mediator.Send(new GetCompaniesQuery { ListParameters = new ListParameters() });

        return await HttpResponseBuilder.BuildAsync(request, result.Companies);
    }
}
