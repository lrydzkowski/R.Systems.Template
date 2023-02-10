using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Services;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Companies.Queries.GetCompany;

namespace R.Systems.Template.Api.AzureFunctions.Functions.Companies;

internal class GetCompanyFunction : FunctionBase<GetCompanyFunction>
{
    public GetCompanyFunction(
        ILogger<GetCompanyFunction> logger,
        RequestPayloadSerializer requestPayloadSerializer,
        HttpResponseBuilder httpResponseBuilder,
        ISender mediator,
        IMapper mapper
    ) : base(logger, requestPayloadSerializer, httpResponseBuilder, mediator, mapper)
    {
    }

    [OpenApiOperation(
        operationId: nameof(GetCompany),
        Summary = nameof(GetCompany),
        Description = "It returns a company."
    )]
    [OpenApiParameter(name: nameof(companyId), Type = typeof(int), Required = true)]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(Company),
        Description = "A company"
    )]
    [Function(nameof(GetCompany))]
    public async Task<HttpResponseData> GetCompany(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "companies/{companyId:int}")]
        HttpRequestData request,
        int companyId
    )
    {
        Logger.LogInformation($"C# Start processing {nameof(GetCompany)} function.");

        GetCompanyQuery query = new() { CompanyId = companyId };
        GetCompanyResult result = await Mediator.Send(query);
        if (result.Company == null)
        {
            return await HttpResponseBuilder.BuildNotFoundAsync(
                request,
                new ErrorInfo
                {
                    PropertyName = "Company",
                    ErrorMessage = "Company doesn't exist.",
                    ErrorCode = "NotExist",
                    AttemptedValue = query
                }
            );
        }

        return await HttpResponseBuilder.BuildAsync(request, result.Company);
    }
}
