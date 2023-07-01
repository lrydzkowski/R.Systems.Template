using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Mappers;
using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Api.AzureFunctions.Services;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using System.Net;

namespace R.Systems.Template.Api.AzureFunctions.Functions.Companies;

internal class CreateCompanyFunction : FunctionBase<CreateCompanyFunction>
{
    public CreateCompanyFunction(
        ILogger<CreateCompanyFunction> logger,
        IRequestPayloadSerializer requestPayloadSerializer,
        IHttpResponseBuilder httpResponseBuilder,
        ISender mediator
    ) : base(logger, requestPayloadSerializer, httpResponseBuilder, mediator)
    {
    }

    [OpenApiOperation(
        operationId: nameof(CreateCompany),
        Summary = nameof(CreateCompany),
        Description = "It creates a company."
    )]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(Company),
        Description = "Create company"
    )]
    [Function(nameof(CreateCompany))]
    public async Task<HttpResponseData> CreateCompany(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "companies")]
        HttpRequestData requestData
    )
    {
        Logger.LogInformation("C# Start processing {FunctionName} function.", nameof(CreateCompany));

        CompanyMapper mapper = new();
        CreateCompanyRequest? request =
            await RequestPayloadSerializer.DeserializeAsync<CreateCompanyRequest>(requestData);
        CreateCompanyCommand command = mapper.ToCreateCommand(request);
        CreateCompanyResult result = await Mediator.Send(command);

        return await HttpResponseBuilder.BuildAsync(requestData, result.Company);
    }
}
