using System.Net;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Mappers;
using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Api.AzureFunctions.Services;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;

namespace R.Systems.Template.Api.AzureFunctions.Functions.Companies;

internal class UpdateCompanyFunction : FunctionBase<UpdateCompanyFunction>
{
    public UpdateCompanyFunction(
        ILogger<UpdateCompanyFunction> logger,
        IRequestPayloadSerializer requestPayloadSerializer,
        IHttpResponseBuilder httpResponseBuilder,
        ISender mediator
    ) : base(logger, requestPayloadSerializer, httpResponseBuilder, mediator)
    {
    }

    [OpenApiOperation(nameof(UpdateCompany), Summary = nameof(UpdateCompany), Description = "It updates a company.")]
    [OpenApiParameter(nameof(companyId), Type = typeof(long), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Company), Description = "Updated company")]
    [Function(nameof(UpdateCompany))]
    public async Task<HttpResponseData> UpdateCompany(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "companies/{companyId:long}")]
        HttpRequestData requestData,
        long companyId
    )
    {
        Logger.LogInformation("C# Start processing {FunctionName} function.", nameof(UpdateCompany));
        CompanyMapper mapper = new();
        UpdateCompanyRequest? request =
            await RequestPayloadSerializer.DeserializeAsync<UpdateCompanyRequest>(requestData);
        UpdateCompanyCommand command = mapper.ToUpdateCommand(request);
        command.CompanyId = companyId;
        UpdateCompanyResult result = await Mediator.Send(command);
        return await HttpResponseBuilder.BuildAsync(requestData, result.Company);
    }
}
