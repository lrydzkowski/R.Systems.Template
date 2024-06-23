using System.Net;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using R.Systems.Template.Api.AzureFunctions.Mappers;
using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Api.AzureFunctions.Services;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;

namespace R.Systems.Template.Api.AzureFunctions.Functions.Companies;

internal class GetCompaniesFunction : FunctionBase<GetCompaniesFunction>
{
    public GetCompaniesFunction(
        ILogger<GetCompaniesFunction> logger,
        IRequestPayloadSerializer requestPayloadSerializer,
        IHttpResponseBuilder httpResponseBuilder,
        ISender mediator
    ) : base(logger, requestPayloadSerializer, httpResponseBuilder, mediator)
    {
    }

    [OpenApiOperation(
        nameof(GetCompanies),
        Summary = nameof(GetCompanies),
        Description = "It returns the list of companies."
    )]
    [OpenApiParameter(nameof(page), Type = typeof(int), Required = true, In = ParameterLocation.Query)]
    [OpenApiParameter(nameof(pageSize), Type = typeof(int), Required = false, In = ParameterLocation.Query)]
    [OpenApiParameter(nameof(sortingFieldName), Type = typeof(string), Required = false, In = ParameterLocation.Query)]
    [OpenApiParameter(nameof(sortingOrder), Type = typeof(string), Required = false, In = ParameterLocation.Query)]
    [OpenApiParameter(nameof(searchQuery), Type = typeof(string), Required = false, In = ParameterLocation.Query)]
    [OpenApiResponseWithBody(
        HttpStatusCode.OK,
        "application/json",
        typeof(ListInfo<Company>),
        Description = "The list of companies"
    )]
    [Function(nameof(GetCompanies))]
    public async Task<HttpResponseData> GetCompanies(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "companies")] HttpRequestData request,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 100,
        string? sortingFieldName = null,
        string sortingOrder = "",
        string? searchQuery = null
    )
    {
        Logger.LogInformation("C# Start processing {FunctionName} function.", nameof(GetCompanies));
        ListMapper mapper = new();
        ListParameters listParameters = mapper.ToListParameter(
            new ListRequest
            {
                Page = page, PageSize = pageSize, SortingFieldName = sortingFieldName, SortingOrder = sortingOrder,
                SearchQuery = searchQuery
            }
        );
        GetCompaniesResult result = await Mediator.Send(
            new GetCompaniesQuery { ListParameters = listParameters },
            cancellationToken
        );
        return await HttpResponseBuilder.BuildAsync(request, result.Companies);
    }
}
