using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Configurations;

internal class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public static readonly int FirstAvailableId = 4;

    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        SetTableName(builder);
        SetPrimaryKey(builder);
        ConfigureRelations(builder);
        ConfigureColumns(builder);
        InitData(builder);
    }

    private void SetTableName(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.ToTable("employee");
    }

    private void SetPrimaryKey(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.HasKey(employee => employee.Id);
    }

    private void ConfigureRelations(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.HasOne(employee => employee.Company)
            .WithMany(parent => parent.Employees)
            .HasForeignKey(employee => employee.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureColumns(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.Property(employee => employee.Id).HasColumnName("id").ValueGeneratedOnAdd().IsRequired();
        builder.Property(employee => employee.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(100);
        builder.Property(employee => employee.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(100);
        builder.Property(employee => employee.CompanyId).HasColumnName("company_id");
    }

    private void InitData(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.HasData(
            new EmployeeEntity { Id = 1, FirstName = "John", LastName = "Doe", CompanyId = 1 },
            new EmployeeEntity { Id = 2, FirstName = "Will", LastName = "Smith", CompanyId = 2 },
            new EmployeeEntity { Id = 3, FirstName = "Jack", LastName = "Parker", CompanyId = 2 }
        );
        builder.Property(user => user.Id).HasIdentityOptions(FirstAvailableId);
    }
}
