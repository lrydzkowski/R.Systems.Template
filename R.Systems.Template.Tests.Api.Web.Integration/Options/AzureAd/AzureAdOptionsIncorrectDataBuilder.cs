using R.Systems.Template.Infrastructure.Azure.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.AzureAd;

internal class AzureAdOptionsIncorrectDataBuilder : IncorrectDataBuilderBase<AzureAdOptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new AzureAdOptionsData { Instance = "" },
                BuildExpectedExceptionMessage(
                    new List<string>
                        { BuildNotEmptyErrorMessage(AzureAdOptions.Position, nameof(AzureAdOptions.Instance)) }
                )
            ),
            BuildParameters(
                2,
                new AzureAdOptionsData { Instance = "  " },
                BuildExpectedExceptionMessage(
                    new List<string>
                        { BuildNotEmptyErrorMessage(AzureAdOptions.Position, nameof(AzureAdOptions.Instance)) }
                )
            ),
            BuildParameters(
                3,
                new AzureAdOptionsData { ClientId = "" },
                BuildExpectedExceptionMessage(
                    new List<string>
                        { BuildNotEmptyErrorMessage(AzureAdOptions.Position, nameof(AzureAdOptions.ClientId)) }
                )
            ),
            BuildParameters(
                4,
                new AzureAdOptionsData { ClientId = "  " },
                BuildExpectedExceptionMessage(
                    new List<string>
                        { BuildNotEmptyErrorMessage(AzureAdOptions.Position, nameof(AzureAdOptions.ClientId)) }
                )
            ),
            BuildParameters(
                5,
                new AzureAdOptionsData { TenantId = "" },
                BuildExpectedExceptionMessage(
                    new List<string>
                        { BuildNotEmptyErrorMessage(AzureAdOptions.Position, nameof(AzureAdOptions.TenantId)) }
                )
            ),
            BuildParameters(
                6,
                new AzureAdOptionsData { TenantId = "  " },
                BuildExpectedExceptionMessage(
                    new List<string>
                        { BuildNotEmptyErrorMessage(AzureAdOptions.Position, nameof(AzureAdOptions.TenantId)) }
                )
            ),
            BuildParameters(
                7,
                new AzureAdOptionsData { Audience = "" },
                BuildExpectedExceptionMessage(
                    new List<string>
                        { BuildNotEmptyErrorMessage(AzureAdOptions.Position, nameof(AzureAdOptions.Audience)) }
                )
            ),
            BuildParameters(
                8,
                new AzureAdOptionsData { Audience = "  " },
                BuildExpectedExceptionMessage(
                    new List<string>
                        { BuildNotEmptyErrorMessage(AzureAdOptions.Position, nameof(AzureAdOptions.Audience)) }
                )
            )
        };
    }
}
