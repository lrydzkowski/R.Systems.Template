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
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;

namespace R.Systems.Template.Api.AzureFunctions.Functions.Companies;

internal class UpdateCompanyFunction : FunctionBase<UpdateCompanyFunction>
{
    public UpdateCompanyFunction(
        ILogger<UpdateCompanyFunction> logger,
        RequestPayloadSerializer requestPayloadSerializer,
        HttpResponseBuilder httpResponseBuilder,
        ISender mediator,
        IMapper mapper
    ) : base(logger, requestPayloadSerializer, httpResponseBuilder, mediator, mapper)
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
        HttpRequestData request,
        int companyId
    )
    {
        Logger.LogInformation($"C# Start processing {nameof(UpdateCompany)} function.");

        UpdateCompanyCommand command = Mapper.Map<UpdateCompanyCommand>(
            await RequestPayloadSerializer.DeserializeAsync<UpdateCompanyRequest>(request)
        );
        command.CompanyId = companyId;
        UpdateCompanyResult result = await Mediator.Send(command);

        return await HttpResponseBuilder.BuildAsync(request, result.Company);
    }
}
