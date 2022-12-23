using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace R.Systems.Template.Infrastructure.Db;

internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        string connectionString = GetConnectionStringFromUserSecrets();
        DbContextOptionsBuilder<AppDbContext> builder = new();
        builder.UseNpgsql(
            connectionString,
            x => x.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
        );

        return new AppDbContext(builder.Options);
    }

    private string GetConnectionStringFromUserSecrets()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<AppDbContext>().Build();
        IConfigurationProvider secretProvider = config.Providers.First();
        if (!secretProvider.TryGet("ConnectionStrings:AppDb", out var connectionString)
            || connectionString == null
            || connectionString.Length == 0)
        {
            throw new Exception("There is no ConnectionStrings:AppDb in user secrets.");
        }

        return connectionString;
    }
}
