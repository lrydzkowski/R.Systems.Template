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
    }
}
