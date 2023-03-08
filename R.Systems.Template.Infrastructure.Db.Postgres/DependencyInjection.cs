﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using R.Systems.Template.Core;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Core.Employees.Commands.CreateEmployee;
using R.Systems.Template.Core.Employees.Commands.UpdateEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using R.Systems.Template.Infrastructure.Db.Postgres.Common.Options;
using R.Systems.Template.Infrastructure.Db.Postgres.Companies.Commands;
using R.Systems.Template.Infrastructure.Db.Postgres.Companies.Queries;
using R.Systems.Template.Infrastructure.Db.Postgres.Employees.Commands;
using R.Systems.Template.Infrastructure.Db.Postgres.Employees.Queries;

namespace R.Systems.Template.Infrastructure.Db.Postgres;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureDbPostgresServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.ConfigureOptions(configuration);
        services.ConfigureAppDbContext();
        services.ConfigureAutoMapper();
        services.ConfigureServices();
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
        services.AddDbContext<AppDbContext>(
            (serviceProvider, options) =>
            {
                ConnectionStringsOptions connectionStrings =
                    serviceProvider.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value;
                options.UseNpgsql(connectionStrings.AppDb);
            }
        );
    }

    private static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection));
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IGetCompanyRepository, GetCompanyRepository>();
        services.AddScoped<IGetCompaniesRepository, GetCompaniesRepository>();
        services.AddScoped<ICreateCompanyRepository, CreateCompanyRepository>();
        services.AddScoped<IUpdateCompanyRepository, UpdateCompanyRepository>();
        services.AddScoped<EmployeeValidator>();
        services.AddScoped<IGetEmployeeRepository, GetEmployeeRepository>();
        services.AddScoped<IGetEmployeesRepository, GetEmployeesRepository>();
        services.AddScoped<ICreateEmployeeRepository, CreateEmployeeRepository>();
        services.AddScoped<IUpdateEmployeeRepository, UpdateEmployeeRepository>();
        services.AddScoped<DbExceptionHandler>();
    }
}