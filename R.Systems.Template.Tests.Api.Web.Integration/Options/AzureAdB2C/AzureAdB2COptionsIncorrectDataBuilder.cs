using R.Systems.Template.Infrastructure.Azure.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.AzureAdB2C;

internal class AzureAdB2COptionsIncorrectDataBuilder : IncorrectDataBuilderBase<AzureAdB2COptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new AzureAdB2COptionsData
                {
                    Instance = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(AzureAdB2COptions.Position, nameof(AzureAdB2COptions.Instance))
                    }
                )
            ),
            BuildParameters(
                2,
                new AzureAdB2COptionsData
                {
                    Instance = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(AzureAdB2COptions.Position, nameof(AzureAdB2COptions.Instance))
                    }
                )
            ),
            BuildParameters(
                3,
                new AzureAdB2COptionsData
                {
                    ClientId = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(AzureAdB2COptions.Position, nameof(AzureAdB2COptions.ClientId))
                    }
                )
            ),
            BuildParameters(
                4,
                new AzureAdB2COptionsData
                {
                    ClientId = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(AzureAdB2COptions.Position, nameof(AzureAdB2COptions.ClientId))
                    }
                )
            ),
            BuildParameters(
                5,
                new AzureAdB2COptionsData
                {
                    Domain = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(AzureAdB2COptions.Position, nameof(AzureAdB2COptions.Domain))
                    }
                )
            ),
            BuildParameters(
                6,
                new AzureAdB2COptionsData
                {
                    Domain = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(AzureAdB2COptions.Position, nameof(AzureAdB2COptions.Domain))
                    }
                )
            ),
            BuildParameters(
                7,
                new AzureAdB2COptionsData
                {
                    SignUpSignInPolicyId = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            AzureAdB2COptions.Position,
                            nameof(AzureAdB2COptions.SignUpSignInPolicyId)
                        )
                    }
                )
            ),
            BuildParameters(
                8,
                new AzureAdB2COptionsData
                {
                    SignUpSignInPolicyId = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            AzureAdB2COptions.Position,
                            nameof(AzureAdB2COptions.SignUpSignInPolicyId)
                        )
                    }
                )
            )
        };
    }
}
