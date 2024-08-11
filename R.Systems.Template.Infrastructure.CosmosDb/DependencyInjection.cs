using Azure.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using R.Systems.Template.Core;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.DeleteCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.DeleteEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using R.Systems.Template.Infrastructure.Azure.Authentication;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Mappers;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Options;
using R.Systems.Template.Infrastructure.CosmosDb.Companies.Commands;
using R.Systems.Template.Infrastructure.CosmosDb.Companies.Queries;
using R.Systems.Template.Infrastructure.CosmosDb.Employees.Commands;
using R.Systems.Template.Infrastructure.CosmosDb.Employees.Queries;
using R.Systems.Template.Infrastructure.CosmosDb.Health;

namespace R.Systems.Template.Infrastructure.CosmosDb;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureCosmosDbServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.ConfigureOptions(configuration);
        services.ConfigureCosmosDbClient();
        services.ConfigureAppDbContext();
        services.ConfigureServices();
        services.AddHealthChecks().AddCheck<CosmosDbHealthCheck>(nameof(CosmosDbHealthCheck));
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<CosmosDbOptions, CosmosDbOptionsValidator>(
            configuration,
            CosmosDbOptions.Position
        );
    }

    private static void ConfigureCosmosDbClient(this IServiceCollection services)
    {
        services.AddAzureClients(
            azureClientFactoryBuilder =>
            {
                azureClientFactoryBuilder.AddClient<CosmosClient, CosmosClientOptions>(
                    (_, serviceProvider) =>
                    {
                        CosmosDbOptions options = serviceProvider
                            .GetRequiredService<IOptions<CosmosDbOptions>>()
                            .Value;

                        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        TokenCredential? tokenCredential = TokenCredentialProvider.Provide(configuration);

                        CosmosClient client = new(options.AccountUri, tokenCredential);

                        return client;
                    }
                );
            }
        );
    }

    private static void ConfigureAppDbContext(this IServiceCollection services)
    {
        services.AddSingleton<AppDbContext>();
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IGetCompanyRepository, GetCompanyRepository>();
        services.AddScoped<IGetCompaniesRepository, GetCompaniesRepository>();
        services.AddScoped<ICreateCompanyRepository, CompanyRepository>();
        services.AddScoped<IUpdateCompanyRepository, CompanyRepository>();
        services.AddScoped<IDeleteCompanyRepository, CompanyRepository>();
        services.AddScoped<IGetEmployeeRepository, GetEmployeeRepository>();
        services.AddScoped<IGetEmployeesRepository, GetEmployeesRepository>();
        services.AddScoped<ICreateEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUpdateEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDeleteEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICompanyMapper, CompanyMapper>();
        services.AddScoped<IEmployeeMapper, EmployeeMapper>();
    }
}
