using R.Systems.Template.Tests.Integration.Common.Options;

namespace R.Systems.Template.Tests.Integration.Options.ConnectionStrings;

internal class ConnectionStringsOptionsIncorrectDataBuilder : IncorrectDataBuilderBase<ConnectionStringsOptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new ConnectionStringsOptionsData
                {
                    AppDb = null
                },
                GetExpectedExceptionMessage(
                    new List<string>
                    {
                        "ConnectionStrings.AppDb: 'AppDb' must not be empty. Severity: Error"
                    }
                )
            ),
            BuildParameters(
                2,
                new ConnectionStringsOptionsData
                {
                    AppDb = "  "
                },
                GetExpectedExceptionMessage(
                    new List<string>
                    {
                        "ConnectionStrings.AppDb: 'AppDb' must not be empty. Severity: Error"
                    }
                )
            )
        };
    }
}
