using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using R.Systems.Template.Core.Common.Errors;
using R.Systems.Template.Core.Common.Extensions;
using System.Net;

namespace R.Systems.Template.Api.Web.Services;

public static class InvalidModelStateService
{
    public const string InvalidErrorCode = "Invalid";

    public static IActionResult InvalidModelStateResponseFactory(ActionContext context)
    {
        List<ErrorInfo> errors = new();
        foreach ((string key, ModelStateEntry modelStateEntry) in context.ModelState)
        {
            if (modelStateEntry.ValidationState == ModelValidationState.Valid)
            {
                continue;
            }

            string propertyName = key.FirstLetterUpperCase();
            string errorMsg = modelStateEntry.Errors.Count == 0
                ? ""
                : string.Join(' ', modelStateEntry.Errors.Select(x => x.ErrorMessage));
            errors.Add(
                new ErrorInfo
                {
                    PropertyName = propertyName,
                    ErrorMessage = errorMsg,
                    AttemptedValue = modelStateEntry.AttemptedValue,
                    ErrorCode = InvalidErrorCode
                }
            );
        }

        return new ObjectResult(errors)
        {
            StatusCode = (int)HttpStatusCode.UnprocessableEntity
        };
    }
}
