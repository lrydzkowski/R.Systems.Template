using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Persistence.Db.Common.Options;
using R.Systems.Template.Persistence.Db.Companies.Queries;

namespace R.Systems.Template.Persistence.Db;

public static class DependencyInjection
{
    public static void AddPersistenceDbService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<ConnectionString>(configuration.GetSection(ConnectionString.Position));
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            ConnectionString connectionString = serviceProvider.GetRequiredService<IOptions<ConnectionString>>().Value;
            options.UseNpgsql(connectionString.AppDb);
        });
        services.AddAutoMapper(typeof(DependencyInjection));
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IGetCompanyRepository, GetCompanyRepository>();
        services.AddScoped<IGetCompaniesRepository, GetCompaniesRepository>();
    }
}
