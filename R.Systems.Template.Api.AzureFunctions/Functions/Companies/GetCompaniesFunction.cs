using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
        RequestPayloadSerializer requestPayloadSerializer,
        HttpResponseBuilder httpResponseBuilder,
        ISender mediator,
        IMapper mapper
    ) : base(logger, requestPayloadSerializer, httpResponseBuilder, mediator, mapper)
    {
    }

    [OpenApiOperation(
        operationId: nameof(GetCompanies),
        Summary = nameof(GetCompanies),
        Description = "It returns the list of companies."
    )]
    [OpenApiParameter(
        name: nameof(page),
        Type = typeof(int),
        Required = true,
        In = ParameterLocation.Query
    )]
    [OpenApiParameter(
        name: nameof(pageSize),
        Type = typeof(int),
        Required = false,
        In = ParameterLocation.Query
    )]
    [OpenApiParameter(
        name: nameof(sortingFieldName),
        Type = typeof(string),
        Required = false,
        In = ParameterLocation.Query
    )]
    [OpenApiParameter(
        name: nameof(sortingOrder),
        Type = typeof(string),
        Required = false,
        In = ParameterLocation.Query
    )]
    [OpenApiParameter(
        name: nameof(searchQuery),
        Type = typeof(string),
        Required = false,
        In = ParameterLocation.Query
    )]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(ListInfo<Company>),
        Description = "The list of companies"
    )]
    [Function(nameof(GetCompanies))]
    public async Task<HttpResponseData> GetCompanies(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "companies")]
        HttpRequestData request,
        int page,
        int pageSize = 100,
        string? sortingFieldName = null,
        string sortingOrder = "",
        string? searchQuery = null
    )
    {
        Logger.LogInformation($"C# Start processing {nameof(GetCompanies)} function.");

        ListParameters listParameters = Mapper.Map<ListParameters>(
            new ListRequest
            {
                Page = page,
                PageSize = pageSize,
                SortingFieldName = sortingFieldName,
                SortingOrder = sortingOrder,
                SearchQuery = searchQuery
            }
        );
        GetCompaniesResult result =
            await Mediator.Send(new GetCompaniesQuery { ListParameters = listParameters });

        return await HttpResponseBuilder.BuildAsync(request, result.Companies);
    }
}
