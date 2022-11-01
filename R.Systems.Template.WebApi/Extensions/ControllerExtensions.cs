using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.WebApi.Extensions;

public static class ControllerExtensions
{
    public static IActionResult ToOkOrNotFound<TResult>(this Result<TResult?> result, ErrorInfo notFoundErrorInfo)
    {
        return result.Match(
            resultData =>
            {
                if (resultData == null)
                {
                    return new NotFoundObjectResult(notFoundErrorInfo);
                }

                return new OkObjectResult(resultData);
            },
            HandleException
        );
    }

    public static IActionResult ToOk<TResult>(this Result<TResult> result)
    {
        return result.Match(
            resultData => new OkObjectResult(resultData),
            HandleException
        );
    }

    public static IActionResult ToOk<TResult, TContract>(this Result<TResult> result, Func<TResult?, TContract> mapper)
    {
        return result.Match(
            resultData => new OkObjectResult(mapper(resultData)),
            HandleException
        );
    }

    public static IActionResult ToActionResult<TResult>(
        this Result<TResult> result,
        Func<TResult?, IActionResult> actionResultResolver
    )
    {
        return result.Match(
            actionResultResolver,
            HandleException
        );
    }

    public static string GetControllerName<T>(this T controller)
    {
        if (controller == null)
        {
            throw new ArgumentNullException(nameof(controller));
        }

        return controller.GetType().Name.Replace("Controller", "");
    }

    private static IActionResult HandleException(Exception? exception)
    {
        if (exception is ValidationException validationException)
        {
            return Return422Result(validationException);
        }

        return new StatusCodeResult(500);
    }

    private static IActionResult Return422Result(ValidationException validationException)
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

        return new UnprocessableEntityObjectResult(errors);
    }
}
