using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Models;
using R.Systems.Template.Api.AzureFunctions.Services;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using System.Net;
using System.Reflection;
using R.Systems.Template.Api.AzureFunctions.Mappers;

namespace R.Systems.Template.Api.AzureFunctions.Functions;

public class GetAppInfoFunction : FunctionBase<GetAppInfoFunction>
{
    public GetAppInfoFunction(
        ILogger<GetAppInfoFunction> logger,
        IRequestPayloadSerializer requestPayloadSerializer,
        IHttpResponseBuilder httpResponseBuilder,
        ISender mediator
    ) : base(logger, requestPayloadSerializer, httpResponseBuilder, mediator)
    {
    }

    [OpenApiOperation(
        operationId: "GetAppInfo",
        Summary = "GetAppInfo",
        Description = "It returns name and version of the application."
    )]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(GetAppInfoResponse),
        Description = "Name and version of the application"
    )]
    [Function(nameof(GetAppInfo))]
    public async Task<HttpResponseData> GetAppInfo(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "app")]
        HttpRequestData request
    )
    {
        Logger.LogInformation("C# Start processing {FunctionName} function.", nameof(GetAppInfo));

        GetAppInfoMapper mapper = new();
        GetAppInfoResult result = await Mediator.Send(
            new GetAppInfoQuery { AppAssembly = Assembly.GetExecutingAssembly() }
        );
        GetAppInfoResponse response = mapper.ToResponse(result);

        return await HttpResponseBuilder.BuildAsync(request, response);
    }
}
