using System.Reflection;
using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Infrastructure.Db.Postgres.Common.Entities;

namespace R.Systems.Template.Infrastructure.Db.Postgres;

internal class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<CompanyEntity> Companies => Set<CompanyEntity>();

    public DbSet<EmployeeEntity> Employees => Set<EmployeeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
