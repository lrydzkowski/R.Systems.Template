using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Common.Configurations;

internal class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        SetTableName(builder);
        SetPrimaryKey(builder);
        ConfigureRelations(builder);
        ConfigureColumns(builder);
    }

    private void SetTableName(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.ToTable(name: "employee");
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
        builder.Property(employee => employee.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(employee => employee.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(employee => employee.LastName)
            .HasColumnName("last_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(employee => employee.CompanyId)
            .HasColumnName("company_id");
    }
}
