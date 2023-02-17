using FluentValidation.Results;

namespace R.Systems.Template.Tests.Core.Integration.Common.Builders;

internal static class ValidationFailureBuilder
{
    public static ValidationFailure BuildEmptyFieldValidationError(string fieldName, string? fieldNameInMsg = null)
    {
        fieldNameInMsg ??= fieldName;

        return new ValidationFailure()
        {
            PropertyName = $"{fieldName}",
            ErrorMessage = $"'{fieldNameInMsg}' must not be empty.",
            ErrorCode = "NotEmptyValidator"
        };
    }

    public static ValidationFailure BuildTooLongFieldValidationError(
        string fieldName,
        int maxLength,
        string? fieldNameInMsg = null
    )
    {
        fieldNameInMsg ??= fieldName;

        return new ValidationFailure()
        {
            PropertyName = $"{fieldName}",
            ErrorMessage =
                $"The length of '{fieldNameInMsg}' must be {maxLength} characters or fewer. You entered {maxLength + 1} characters.",
            ErrorCode = "MaximumLengthValidator"
        };
    }
}
