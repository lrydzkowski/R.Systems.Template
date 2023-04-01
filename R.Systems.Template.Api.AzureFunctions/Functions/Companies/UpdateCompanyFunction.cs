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

    [OpenApiOperation(
        operationId: nameof(UpdateCompany),
        Summary = nameof(UpdateCompany),
        Description = "It updates a company."
    )]
    [OpenApiParameter(name: nameof(companyId), Type = typeof(int), Required = true)]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(Company),
        Description = "Updated company"
    )]
    [Function(nameof(UpdateCompany))]
    public async Task<HttpResponseData> UpdateCompany(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "companies/{companyId:int}")]
        HttpRequestData requestData,
        int companyId
    )
    {
        Logger.LogInformation($"C# Start processing {nameof(UpdateCompany)} function.");

        CompanyMapper mapper = new();
        UpdateCompanyRequest? request =
            await RequestPayloadSerializer.DeserializeAsync<UpdateCompanyRequest>(requestData);
        UpdateCompanyCommand command = mapper.ToUpdateCommand(request);
        command.CompanyId = companyId;
        UpdateCompanyResult result = await Mediator.Send(command);

        return await HttpResponseBuilder.BuildAsync(requestData, result.Company);
    }
}
