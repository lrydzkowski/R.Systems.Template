namespace R.Systems.Template.Tests.Integration.Common.Options;

internal abstract class IncorrectDataBuilderBase<T> where T : IOptionsData
{
    protected static string GetExpectedExceptionMessage(List<string> exceptionMessageParts)
    {
        exceptionMessageParts = exceptionMessageParts.Select(x => $" -- {x}").ToList();
        exceptionMessageParts.Insert(0, "App settings - Validation failed: ");

        return string.Join(Environment.NewLine, exceptionMessageParts);
    }

    protected static object[] BuildParameters(int id, T data, string expectedExceptionMessage)
    {
        return new object[] { id, data.ConvertToInMemoryCollection(), expectedExceptionMessage };
    }
}
