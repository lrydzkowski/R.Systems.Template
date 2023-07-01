using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Services;
using R.Systems.Template.Core.Companies.Commands.DeleteCompany;
using System.Net;

namespace R.Systems.Template.Api.AzureFunctions.Functions.Companies;

public class DeleteCompanyFunction : FunctionBase<DeleteCompanyFunction>
{
    public DeleteCompanyFunction(
        ILogger<DeleteCompanyFunction> logger,
        IRequestPayloadSerializer requestPayloadSerializer,
        IHttpResponseBuilder httpResponseBuilder,
        ISender mediator
    ) : base(logger, requestPayloadSerializer, httpResponseBuilder, mediator)
    {
    }

    [OpenApiOperation(
        operationId: nameof(DeleteCompany),
        Summary = nameof(DeleteCompany),
        Description = "It deletes a company."
    )]
    [OpenApiParameter(name: nameof(companyId), Type = typeof(int), Required = true)]
    [OpenApiResponseWithoutBody(
        statusCode: HttpStatusCode.NoContent,
        Description = "Company deleted"
    )]
    [Function(nameof(DeleteCompany))]
    public async Task<HttpResponseData> DeleteCompany(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "companies/{companyId:int}")]
        HttpRequestData requestData,
        int companyId
    )
    {
        Logger.LogInformation("C# Start processing {FunctionName} function.", nameof(DeleteCompany));

        DeleteCompanyCommand command = new() { CompanyId = companyId };
        await Mediator.Send(command);

        return HttpResponseBuilder.BuildNoContent(requestData);
    }
}
