using System.Net;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Services;
using R.Systems.Template.Core.Common.Errors;

namespace R.Systems.Template.Api.AzureFunctions.Middleware;

public class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<Program> _logger;
    private readonly CustomJsonSerializer _jsonSerializer;

    public ExceptionHandlingMiddleware(ILogger<Program> logger, CustomJsonSerializer jsonSerializer)
    {
        _logger = logger;
        _jsonSerializer = jsonSerializer;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            switch (exception.InnerException)
            {
                case ValidationException validationException:
                    await HandleValidationExceptionAsync(context, validationException);
                    return;
            }

            _logger.LogError($"Something went wrong: {exception}");
            await HandleExceptionAsync(context);
        }
    }

    private async Task HandleValidationExceptionAsync(FunctionContext context, ValidationException validationException)
    {
        IEnumerable<ErrorInfo> errors = validationException.Errors.Select(
                x => new ErrorInfo
                {
                    PropertyName = x.PropertyName,
                    ErrorMessage = x.ErrorMessage,
                    AttemptedValue = x.AttemptedValue,
                    ErrorCode = x.ErrorCode
                }
            )
            .AsEnumerable();
        string errorsSerialized = _jsonSerializer.Serialize(errors);

        await CreateResponse(context, HttpStatusCode.UnprocessableEntity, errorsSerialized);
    }

    private async Task HandleExceptionAsync(FunctionContext context)
    {
        await CreateResponse(context, HttpStatusCode.InternalServerError);
    }

    private async Task CreateResponse(FunctionContext context, HttpStatusCode statusCode, string? response = null)
    {
        HttpRequestData? httpRequestData = await context.GetHttpRequestDataAsync();

        if (httpRequestData != null)
        {
            HttpResponseData newHttpResponse = httpRequestData.CreateResponse(statusCode);
            newHttpResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
            if (response != null)
            {
                await newHttpResponse.WriteStringAsync(response);
            }

            InvocationResult invocationResult = context.GetInvocationResult();

            OutputBindingData<HttpResponseData>? httpOutputBindingFromMultipleOutputBindings =
                GetHttpOutputBindingFromMultipleOutputBinding(context);
            if (httpOutputBindingFromMultipleOutputBindings is not null)
            {
                httpOutputBindingFromMultipleOutputBindings.Value = newHttpResponse;
            }
            else
            {
                invocationResult.Value = newHttpResponse;
            }
        }
    }

    private OutputBindingData<HttpResponseData>? GetHttpOutputBindingFromMultipleOutputBinding(FunctionContext context)
    {
        return context.GetOutputBindings<HttpResponseData>()
            .FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");
    }
}
