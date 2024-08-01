using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Template.Infrastructure.SqlServerDb.Common.Entities;

namespace R.Systems.Template.Infrastructure.SqlServerDb.Common.Configurations;

internal class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<CompanyEntity>
{
    public void Configure(EntityTypeBuilder<CompanyEntity> builder)
    {
        SetTableName(builder);
        SetPrimaryKey(builder);
        ConfigureColumns(builder);
        InitData(builder);
    }

    private void SetTableName(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder.ToTable("company");
    }

    private void SetPrimaryKey(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder.HasKey(company => company.Id);
    }

    private void ConfigureColumns(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder.Property(company => company.Id).HasColumnName("id").IsRequired();
        builder.Property(company => company.Name).HasColumnName("name").IsRequired().HasMaxLength(200);
        builder.HasIndex(company => company.Name).IsUnique();
    }

    private void InitData(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder.HasData(
            new CompanyEntity { Id = new Guid("9e27c3b4-bf21-4ffe-bdbb-919a2fc9e2cc"), Name = "Meta" },
            new CompanyEntity { Id = new Guid("636786f1-e5aa-4a87-9c7d-e604a92f08f5"), Name = "Google" }
        );
    }
}
