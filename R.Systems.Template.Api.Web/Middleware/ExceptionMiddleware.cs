using System.Net;
using System.Net.Mime;
using System.Text.Json;
using FluentValidation;
using R.Systems.Template.Core.Common.Errors;

namespace R.Systems.Template.Api.Web.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<Program> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<Program> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException validationException)
        {
            await HandleValidationExceptionAsync(httpContext, validationException);
        }
        catch (OperationCanceledException exception)
        {
            _logger.LogWarning(exception, "Operation was cancelled");
            HandleException(httpContext);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong");
            HandleException(httpContext);
        }
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException validationException)
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
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        await context.Response.WriteAsJsonAsync(errors, jsonSerializerOptions);
    }

    private void HandleException(HttpContext context)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    }
}
