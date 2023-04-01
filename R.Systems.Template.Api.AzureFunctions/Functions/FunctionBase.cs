using MediatR;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Services;

namespace R.Systems.Template.Api.AzureFunctions.Functions;

public class FunctionBase<T>
{
    public FunctionBase(
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

    protected ILogger<T> Logger { get; }
    protected IRequestPayloadSerializer RequestPayloadSerializer { get; }
    protected IHttpResponseBuilder HttpResponseBuilder { get; }
    protected ISender Mediator { get; }
}
