using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Api.AzureFunctions.Services;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;

namespace R.Systems.Template.Api.AzureFunctions.Functions.Companies;

internal class CreateCompanyFunction : FunctionBase<CreateCompanyFunction>
{
    public CreateCompanyFunction(
        ILogger<CreateCompanyFunction> logger,
        RequestPayloadSerializer requestPayloadSerializer,
        HttpResponseBuilder httpResponseBuilder,
        ISender mediator,
        IMapper mapper
    ) : base(logger, requestPayloadSerializer, httpResponseBuilder, mediator, mapper)
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
        HttpRequestData request
    )
    {
        Logger.LogInformation($"C# Start processing {nameof(CreateCompany)} function.");

        CreateCompanyCommand command = Mapper.Map<CreateCompanyCommand>(
            await RequestPayloadSerializer.DeserializeAsync<CreateCompanyRequest>(request)
        );
        CreateCompanyResult result = await Mediator.Send(command);

        return await HttpResponseBuilder.BuildAsync(request, result.Company);
    }
}
