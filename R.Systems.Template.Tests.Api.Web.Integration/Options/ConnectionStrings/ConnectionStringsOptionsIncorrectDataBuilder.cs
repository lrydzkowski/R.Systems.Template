using R.Systems.Template.Infrastructure.Db.Common.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.ConnectionStrings;

internal class ConnectionStringsOptionsIncorrectDataBuilder : IncorrectDataBuilderBase<ConnectionStringsOptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        string errorMessage =
            $"Either '{nameof(ConnectionStringsOptions.AppSqlServerDb)}' or '{nameof(ConnectionStringsOptions.AppPostgresDb)}' must not be empty.";

        return new List<object[]>
        {
            BuildParameters(
                1,
                new ConnectionStringsOptionsData
                {
                    AppSqlServerDb = "",
                    AppPostgresDb = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildErrorMessage(ConnectionStringsOptions.Position, errorMessage)
                    }
                )
            ),
            BuildParameters(
                2,
                new ConnectionStringsOptionsData
                {
                    AppSqlServerDb = "  ",
                    AppPostgresDb = " "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildErrorMessage(ConnectionStringsOptions.Position, errorMessage)
                    }
                )
            ),
            BuildParameters(
                3,
                new ConnectionStringsOptionsData
                {
                    AppSqlServerDb = null,
                    AppPostgresDb = null
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildErrorMessage(ConnectionStringsOptions.Position, errorMessage)
                    }
                )
            )
        };
    }
}
