using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
using R.Systems.Template.Infrastructure.MongoDb.Common.Options;
using R.Systems.Template.Infrastructure.MongoDb.Companies.Commands;
using R.Systems.Template.Infrastructure.MongoDb.Companies.Queries;
using R.Systems.Template.Infrastructure.MongoDb.Employees.Commands;
using R.Systems.Template.Infrastructure.MongoDb.Employees.Queries;
using R.Systems.Template.Infrastructure.MongoDb.Health;

namespace R.Systems.Template.Infrastructure.MongoDb;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureMongoDbServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.ConfigureOptions(configuration);
        services.ConfigureAppDbContext();
        services.ConfigureServices();
        services.AddHealthChecks().AddCheck<MongoDbHealthCheck>(nameof(MongoDbHealthCheck));
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<ConnectionStringsOptions, ConnectionStringsOptionsValidator>(
            configuration,
            ConnectionStringsOptions.Position
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
    }
}
