using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Items;

namespace R.Systems.Template.Infrastructure.CosmosDb.Health;

internal class CosmosDbHealthCheck : IHealthCheck
{
    private readonly AppDbContext _appDbContext;

    public CosmosDbHealthCheck(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new()
    )
    {
        try
        {
            Container? container = _appDbContext.Database.GetContainer(CompanyItem.ContainerName);
            await container.ReadContainerAsync(cancellationToken: cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
