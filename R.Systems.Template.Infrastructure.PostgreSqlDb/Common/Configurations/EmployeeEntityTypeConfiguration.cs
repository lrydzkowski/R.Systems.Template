using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Configurations;

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
                Id = new Guid("84b096f7-68a1-47a8-9e6a-8cfd79f0f069"), FirstName = "John", LastName = "Doe",
                CompanyId = new Guid("31b04626-ed12-4d79-b3d6-1430a72000d5")
            },
            new EmployeeEntity
            {
                Id = new Guid("ab189e89-7007-43bf-85d1-b1cc3c69c503"), FirstName = "Will", LastName = "Smith",
                CompanyId = new Guid("9427a96c-a0b6-461c-814c-9c3c2bb6ff80")
            },
            new EmployeeEntity
            {
                Id = new Guid("b82f922b-784c-40e1-b03b-476a0b447dca"), FirstName = "Jack", LastName = "Parker",
                CompanyId = new Guid("9427a96c-a0b6-461c-814c-9c3c2bb6ff80")
            }
        );
    }
}
