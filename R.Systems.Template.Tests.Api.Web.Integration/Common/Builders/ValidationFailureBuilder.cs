using FluentValidation.Results;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Builders;

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

    public static ValidationFailure BuildExactLengthFieldValidationError(
        string fieldName,
        int expectedLength,
        int enteredLength,
        string? fieldNameInMsg = null
    )
    {
        fieldNameInMsg ??= fieldName;

        return new ValidationFailure()
        {
            PropertyName = $"{fieldName}",
            ErrorMessage =
                $"'{fieldNameInMsg}' must be {expectedLength} characters in length. You entered {enteredLength} characters.",
            ErrorCode = ""
        };
    }

    public static ValidationFailure BuildEmailIsNotValidValidationError()
    {
        return new ValidationFailure()
        {
            PropertyName = "Email",
            ErrorMessage = "'Email' is not a valid email address.",
            ErrorCode = ""
        };
    }
}
