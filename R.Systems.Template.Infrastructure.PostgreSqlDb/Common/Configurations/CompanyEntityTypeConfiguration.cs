using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Configurations;

internal class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<CompanyEntity>
{
    public static readonly int FirstAvailableId = 3;

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
        builder.Property(company => company.Id).HasColumnName("id").ValueGeneratedOnAdd().IsRequired();
        builder.Property(company => company.Name).HasColumnName("name").IsRequired().HasMaxLength(200);
        builder.HasIndex(company => company.Name).IsUnique();
    }

    private void InitData(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder.HasData(new CompanyEntity { Id = 1, Name = "Meta" }, new CompanyEntity { Id = 2, Name = "Google" });
        builder.Property(user => user.Id).HasIdentityOptions(FirstAvailableId);
    }
}
