using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Common.Configurations;

internal class CompanyConfiguration : IEntityTypeConfiguration<CompanyEntity>
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
        builder.ToTable(name: "company");
    }

    private void SetPrimaryKey(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder.HasKey(company => company.Id);
    }

    private void ConfigureColumns(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder.Property(company => company.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(company => company.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(company => company.Name)
            .IsUnique();
    }

    private void InitData(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder.HasData(
            new()
            {
                Id = 1,
                Name = "Meta"
            },
            new()
            {
                Id = 2,
                Name = "Google"
            }
        );
        builder.Property(user => user.Id).HasIdentityOptions(startValue: 3);
    }
}
