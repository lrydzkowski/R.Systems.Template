using R.Systems.Template.Persistence.Db.Common.Options;
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
                    AppDb = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            ConnectionStringsOptions.Position,
                            nameof(ConnectionStringsOptions.AppDb)
                        )
                    }
                )
            ),
            BuildParameters(
                2,
                new ConnectionStringsOptionsData
                {
                    AppDb = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            ConnectionStringsOptions.Position,
                            nameof(ConnectionStringsOptions.AppDb)
                        )
                    }
                )
            )
        };
    }
}
