using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Template.Infrastructure.SqlServerDb.Common.Entities;

namespace R.Systems.Template.Infrastructure.SqlServerDb.Common.Configurations;

internal class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
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
        builder.Property(employee => employee.Id).HasColumnName("id").IsRequired();
        builder.Property(employee => employee.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(100);
        builder.Property(employee => employee.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(100);
        builder.Property(employee => employee.CompanyId).HasColumnName("company_id");
    }

    private void InitData(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.HasData(
            new EmployeeEntity
            {
                Id = new Guid("424c6d87-3c97-4eb9-9a3e-0abbc7547683"), FirstName = "John", LastName = "Doe",
                CompanyId = new Guid("9e27c3b4-bf21-4ffe-bdbb-919a2fc9e2cc")
            },
            new EmployeeEntity
            {
                Id = new Guid("878ae60f-c657-4465-8920-9d7d34f757ed"), FirstName = "Will", LastName = "Smith",
                CompanyId = new Guid("636786f1-e5aa-4a87-9c7d-e604a92f08f5")
            },
            new EmployeeEntity
            {
                Id = new Guid("194ac2c8-72e3-4c63-8302-0217b9cc86b6"), FirstName = "Jack", LastName = "Parker",
                CompanyId = new Guid("636786f1-e5aa-4a87-9c7d-e604a92f08f5")
            }
        );
    }
}
