using R.Systems.Template.Api.Web.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.HealthCheck;

internal class HealthCheckOptionsIncorrectDataBuilder : IncorrectDataBuilderBase<HealthCheckOptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new HealthCheckOptionsData { ApiKey = "" },
                BuildExpectedExceptionMessage(
                    new List<string>
                        { BuildNotEmptyErrorMessage(HealthCheckOptions.Position, nameof(HealthCheckOptions.ApiKey)) }
                )
            ),
            BuildParameters(
                2,
                new HealthCheckOptionsData { ApiKey = " " },
                BuildExpectedExceptionMessage(
                    new List<string>
                        { BuildNotEmptyErrorMessage(HealthCheckOptions.Position, nameof(HealthCheckOptions.ApiKey)) }
                )
            )
        };
    }
}
