using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Items;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Options;

namespace R.Systems.Template.Infrastructure.CosmosDb;

internal class AppDbContext
{
    public AppDbContext(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options)
    {
        Database = cosmosClient.GetDatabase(options.Value.DatabaseName);
        CompaniesContainer = Database.GetContainer(CompanyItem.ContainerName);
        EmployeesContainers = Database.GetContainer(EmployeeItem.ContainerName);
    }

    public Database Database { get; }

    public Container CompaniesContainer { get; }

    public Container EmployeesContainers { get; }
}
