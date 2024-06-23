using MediatR;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Services;

namespace R.Systems.Template.Api.AzureFunctions.Functions;

public class FunctionBase<T>
{
    protected readonly IHttpResponseBuilder HttpResponseBuilder;
    protected readonly ILogger<T> Logger;
    protected readonly ISender Mediator;
    protected readonly IRequestPayloadSerializer RequestPayloadSerializer;

    protected FunctionBase(
        ILogger<T> logger,
        IRequestPayloadSerializer requestPayloadSerializer,
        IHttpResponseBuilder httpResponseBuilder,
        ISender mediator
    )
    {
        Logger = logger;
        RequestPayloadSerializer = requestPayloadSerializer;
        HttpResponseBuilder = httpResponseBuilder;
        Mediator = mediator;
    }
}
