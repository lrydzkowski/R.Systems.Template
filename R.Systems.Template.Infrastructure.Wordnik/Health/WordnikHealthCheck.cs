using Microsoft.Extensions.Diagnostics.HealthChecks;
using R.Systems.Template.Infrastructure.Wordnik.Common.Api;

namespace R.Systems.Template.Infrastructure.Wordnik.Health;

internal class WordnikHealthCheck : IHealthCheck
{
    private readonly WordApi _wordApi;

    public WordnikHealthCheck(WordApi wordApi)
    {
        _wordApi = wordApi;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new()
    )
    {
        try
        {
            await _wordApi.GetRandomWordAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
