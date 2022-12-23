﻿using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using R.Systems.Template.Api.AzureFunctions.Services;
using R.Systems.Template.Core.Common.Errors;
using System.Net;

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
        catch (ValidationException validationException)
        {
            await HandleValidationExceptionAsync(context, validationException);
        }
        catch (Exception exception)
        {
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
        var httpReqData = await context.GetHttpRequestDataAsync();

        if (httpReqData != null)
        {
            var newHttpResponse = httpReqData.CreateResponse(statusCode);
            if (response != null)
            {
                await newHttpResponse.WriteStringAsync(response);
            }

            var invocationResult = context.GetInvocationResult();

            var httpOutputBindingFromMultipleOutputBindings = GetHttpOutputBindingFromMultipleOutputBinding(context);
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
        var httpOutputBinding = context.GetOutputBindings<HttpResponseData>()
            .FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");

        return httpOutputBinding;
    }
}