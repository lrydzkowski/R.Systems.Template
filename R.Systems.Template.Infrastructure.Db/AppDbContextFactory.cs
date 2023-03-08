using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using R.Systems.Template.Infrastructure.Db.Common.Options;

namespace R.Systems.Template.Infrastructure.Db;

internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        string connectionString = GetConnectionStringFromUserSecrets();
        DbContextOptionsBuilder<AppDbContext> builder = new();
        builder.UseSqlServer(
            connectionString,
            x => x.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
        );

        return new AppDbContext(builder.Options);
    }

    private string GetConnectionStringFromUserSecrets()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<AppDbContext>().Build();
        IConfigurationProvider secretProvider = config.Providers.First();
        if (!secretProvider.TryGet(
                $"{ConnectionStringsOptions.Position}:{nameof(ConnectionStringsOptions.AppDb)}",
                out string? connectionString
            )
            || connectionString == null
            || connectionString.Length == 0)
        {
            throw new Exception(
                $"There is no {ConnectionStringsOptions.Position}:{nameof(ConnectionStringsOptions.AppDb)} in user secrets."
            );
        }

        return connectionString;
    }
}
