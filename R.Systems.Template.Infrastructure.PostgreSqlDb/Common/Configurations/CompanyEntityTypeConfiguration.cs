using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Configurations;

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
            new CompanyEntity { Id = new Guid("31b04626-ed12-4d79-b3d6-1430a72000d5"), Name = "Meta" },
            new CompanyEntity { Id = new Guid("9427a96c-a0b6-461c-814c-9c3c2bb6ff80"), Name = "Google" }
        );
        builder.Property(user => user.Id);
    }
}
