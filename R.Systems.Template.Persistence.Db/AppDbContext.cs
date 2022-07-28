using Microsoft.EntityFrameworkCore;
using R.Systems.Template.Persistence.Db.Common.Entities;
using System.Reflection;

namespace R.Systems.Template.Persistence.Db;

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
