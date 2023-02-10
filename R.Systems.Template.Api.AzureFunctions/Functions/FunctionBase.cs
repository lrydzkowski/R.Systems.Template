using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Services;

namespace R.Systems.Template.Api.AzureFunctions.Functions;

public class FunctionBase<T>
{
    protected FunctionBase(
        ILogger<T> logger,
        RequestPayloadSerializer requestPayloadSerializer,
        HttpResponseBuilder httpResponseBuilder,
        ISender mediator,
        IMapper mapper
    )
    {
        Logger = logger;
        RequestPayloadSerializer = requestPayloadSerializer;
        HttpResponseBuilder = httpResponseBuilder;
        Mediator = mediator;
        Mapper = mapper;
    }

    protected ILogger<T> Logger { get; }
    protected RequestPayloadSerializer RequestPayloadSerializer { get; }
    protected HttpResponseBuilder HttpResponseBuilder { get; }
    protected ISender Mediator { get; }
    protected IMapper Mapper { get; }
}
