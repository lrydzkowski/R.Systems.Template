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
                new ConnectionStringsOptionsData { AppPostgreSqlDb = "" },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            ConnectionStringsOptions.Position,
                            nameof(ConnectionStringsOptions.AppPostgreSqlDb)
                        )
                    }
                )
            ),
            BuildParameters(
                2,
                new ConnectionStringsOptionsData { AppPostgreSqlDb = " " },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            ConnectionStringsOptions.Position,
                            nameof(ConnectionStringsOptions.AppPostgreSqlDb)
                        )
                    }
                )
            ),
            BuildParameters(
                3,
                new ConnectionStringsOptionsData { AppPostgreSqlDb = null },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            ConnectionStringsOptions.Position,
                            nameof(ConnectionStringsOptions.AppPostgreSqlDb)
                        )
                    }
                )
            )
        };
    }
}
