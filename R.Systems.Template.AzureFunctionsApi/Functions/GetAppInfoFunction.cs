using AutoMapper;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using R.Systems.Template.AzureFunctionsApi.Api;
using R.Systems.Template.AzureFunctionsApi.Services;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using System.Net;
using System.Reflection;

namespace R.Systems.Template.AzureFunctionsApi.Functions
{
    public class GetAppInfoFunction : FunctionBase<GetAppInfoFunction>
    {
        public GetAppInfoFunction(
            ILogger<GetAppInfoFunction> logger,
            HttpResponseBuilder httpResponseBuilder,
            ISender mediator,
            IMapper mapper
        ) : base(logger, httpResponseBuilder, mediator, mapper)
        {
        }

        [OpenApiOperation(
            operationId: "GetAppInfo",
            Summary = "GetAppInfo",
            Description = "It returns name and version of the application.",
            Visibility = OpenApiVisibilityType.Important
        )]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(GetAppInfoResponse),
            Description = "Name and version of the application"
        )]
        [Function(nameof(GetAppInfo))]
        public async Task<HttpResponseData> GetAppInfo(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
            HttpRequestData request
        )
        {
            Logger.LogInformation("C# Start processing GetAppInfo function.");

            GetAppInfoResult result = await Mediator.Send(
                new GetAppInfoQuery { AppAssembly = Assembly.GetExecutingAssembly() }
            );
            GetAppInfoResponse response = Mapper.Map<GetAppInfoResponse>(result);

            return await HttpResponseBuilder.BuildAsync(request, response);
        }
    }
}
