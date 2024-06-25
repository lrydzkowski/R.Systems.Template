using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;

namespace R.Systems.Template.Infrastructure.MongoDb.Health;

internal class MongoDbHealthCheck : IHealthCheck
{
    private readonly AppDbContext _appDbContext;

    public MongoDbHealthCheck(AppDbContext appDbContext)
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
            IMongoQueryable<CompanyDocument> query = _appDbContext.Companies.AsQueryable().Take(10);
            await query.ToListAsync(cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
