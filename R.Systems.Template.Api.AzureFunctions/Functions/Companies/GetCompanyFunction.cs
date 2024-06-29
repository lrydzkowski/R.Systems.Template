using System.Net;
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
        IRequestPayloadSerializer requestPayloadSerializer,
        IHttpResponseBuilder httpResponseBuilder,
        ISender mediator
    ) : base(logger, requestPayloadSerializer, httpResponseBuilder, mediator)
    {
    }

    [OpenApiOperation(nameof(GetCompany), Summary = nameof(GetCompany), Description = "It returns a company.")]
    [OpenApiParameter(nameof(companyId), Type = typeof(long), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Company), Description = "A company")]
    [Function(nameof(GetCompany))]
    public async Task<HttpResponseData> GetCompany(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "companies/{companyId:long}")]
        HttpRequestData request,
        long companyId,
        CancellationToken cancellationToken
    )
    {
        Logger.LogInformation("C# Start processing {FunctionName} function.", nameof(GetCompany));
        GetCompanyQuery query = new()
        {
            CompanyId = companyId
        };
        GetCompanyResult result = await Mediator.Send(query, cancellationToken);
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
