using R.Systems.Template.Tests.Integration.Common.Options;

namespace R.Systems.Template.Tests.Integration.Options.AzureAd;

internal class AzureAdOptionsIncorrectDataBuilder : IncorrectDataBuilderBase<AzureAdOptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new AzureAdOptionsData
                {
                    Instance = null
                },
                GetExpectedExceptionMessage(new List<string>
                {
                    "AzureAd.Instance: 'Instance' must not be empty. Severity: Error"
                })
            ),
            BuildParameters(
                2,
                new AzureAdOptionsData
                {
                    Instance = "  "
                },
                GetExpectedExceptionMessage(new List<string>
                {
                    "AzureAd.Instance: 'Instance' must not be empty. Severity: Error"
                })
            ),
            BuildParameters(
                3,
                new AzureAdOptionsData
                {
                    Domain = null
                },
                GetExpectedExceptionMessage(new List<string>
                {
                    "AzureAd.Domain: 'Domain' must not be empty. Severity: Error"
                })
            ),
            BuildParameters(
                4,
                new AzureAdOptionsData
                {
                    Domain = "  "
                },
                GetExpectedExceptionMessage(new List<string>
                {
                    "AzureAd.Domain: 'Domain' must not be empty. Severity: Error"
                })
            ),
            BuildParameters(
                5,
                new AzureAdOptionsData
                {
                    ClientId = null
                },
                GetExpectedExceptionMessage(new List<string>
                {
                    "AzureAd.ClientId: 'ClientId' must not be empty. Severity: Error"
                })
            ),
            BuildParameters(
                6,
                new AzureAdOptionsData
                {
                    ClientId = "  "
                },
                GetExpectedExceptionMessage(new List<string>
                {
                    "AzureAd.ClientId: 'ClientId' must not be empty. Severity: Error"
                })
            ),
            BuildParameters(
                7,
                new AzureAdOptionsData
                {
                    SignUpSignInPolicyId = null
                },
                GetExpectedExceptionMessage(new List<string>
                {
                    "AzureAd.SignUpSignInPolicyId: 'SignUpSignInPolicyId' must not be empty. Severity: Error"
                })
            ),
            BuildParameters(
                8,
                new AzureAdOptionsData
                {
                    SignUpSignInPolicyId = "  "
                },
                GetExpectedExceptionMessage(new List<string>
                {
                    "AzureAd.SignUpSignInPolicyId: 'SignUpSignInPolicyId' must not be empty. Severity: Error"
                })
            )
        };
    }
}
