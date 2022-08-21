using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Persistence.Db.Common.Options;
using R.Systems.Template.Persistence.Db.Companies.Commands;
using R.Systems.Template.Persistence.Db.Companies.Queries;

namespace R.Systems.Template.Persistence.Db;

public static class DependencyInjection
{
    public static void AddPersistenceDbService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<ConnectionStrings>(configuration.GetSection(ConnectionStrings.Position));
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            ConnectionStrings connectionStrings = serviceProvider.GetRequiredService<IOptions<ConnectionStrings>>().Value;
            options.UseNpgsql(connectionStrings.AppDb);
        });
        services.AddAutoMapper(typeof(DependencyInjection));
        services.AddRepositories();
        services.AddScoped<DbExceptionHandler>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IGetCompanyRepository, GetCompanyRepository>();
        services.AddScoped<IGetCompaniesRepository, GetCompaniesRepository>();
        services.AddScoped<ICreateCompanyRepository, CreateCompanyRepository>();
        services.AddScoped<IUpdateCompanyRepository, UpdateCompanyRepository>();
    }
}
