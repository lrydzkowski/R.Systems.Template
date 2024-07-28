using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.ConnectionStrings;

internal class ConnectionStringsOptionsIncorrectDataBuilder : IncorrectDataBuilderBase<ConnectionStringsOptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new ConnectionStringsOptionsData { AppPostgresDb = "" },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            ConnectionStringsOptions.Position,
                            nameof(ConnectionStringsOptions.AppPostgresDb)
                        )
                    }
                )
            ),
            BuildParameters(
                2,
                new ConnectionStringsOptionsData { AppPostgresDb = " " },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            ConnectionStringsOptions.Position,
                            nameof(ConnectionStringsOptions.AppPostgresDb)
                        )
                    }
                )
            ),
            BuildParameters(
                3,
                new ConnectionStringsOptionsData { AppPostgresDb = null },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            ConnectionStringsOptions.Position,
                            nameof(ConnectionStringsOptions.AppPostgresDb)
                        )
                    }
                )
            )
        };
    }
}
