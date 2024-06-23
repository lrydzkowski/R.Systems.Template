using R.Systems.Template.Api.Web.Options;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Template.Tests.Api.Web.Integration.Options.HealthCheck;

internal class HealthCheckOptionsData : HealthCheckOptions, IOptionsData
{
    public HealthCheckOptionsData()
    {
        ApiKey = "2211D1E7-B1C8-4604-B4FD-E9287F27C5BF";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(ApiKey)}"] = ApiKey
        };
    }
}
