namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

internal abstract class IncorrectDataBuilderBase<T>
    where T : IOptionsData
{
    protected static string BuildExpectedExceptionMessage(List<string> exceptionMessageParts)
    {
        exceptionMessageParts = exceptionMessageParts.Select(x => $" -- {x}").ToList();
        exceptionMessageParts.Insert(0, "App settings - Validation failed: ");
        return string.Join(Environment.NewLine, exceptionMessageParts);
    }

    protected static object[] BuildParameters(int id, T data, string expectedExceptionMessage)
    {
        return new object[]
        {
            id,
            data.ConvertToInMemoryCollection(),
            expectedExceptionMessage
        };
    }

    protected static string BuildNotEmptyErrorMessage(string position, string propertyName)
    {
        return $"{position}.{propertyName}: '{propertyName}' must not be empty. Severity: Error";
    }

    protected static string BuildErrorMessage(string property, string errorMsg)
    {
        return $"{property}: {errorMsg} Severity: Error";
    }
}
