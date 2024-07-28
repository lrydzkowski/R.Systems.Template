using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Options;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb;

internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<AppDbContext> builder = GetDbContextOptionsBuilder();
        return new AppDbContext(builder.Options);
    }

    private DbContextOptionsBuilder<AppDbContext> GetDbContextOptionsBuilder()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<AppDbContext>().Build();
        IConfigurationProvider secretProvider = config.Providers.First();
        DbContextOptionsBuilder<AppDbContext> builder = new();
        string? postgresConnectionString = GetConnectionString(
            secretProvider,
            nameof(ConnectionStringsOptions.AppPostgresDb)
        );
        if (IsConnectionStringCorrect(postgresConnectionString))
        {
            builder.UseNpgsql(
                postgresConnectionString,
                x => x.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            );
            return builder;
        }

        throw new Exception("There is no connection string in user secrets.");
    }

    private string? GetConnectionString(IConfigurationProvider secretProvider, string optionName)
    {
        secretProvider.TryGet($"{ConnectionStringsOptions.Position}:{optionName}", out string? connectionString);

        return connectionString;
    }

    private bool IsConnectionStringCorrect(string? connectionString)
    {
        return !string.IsNullOrEmpty(connectionString);
    }
}
