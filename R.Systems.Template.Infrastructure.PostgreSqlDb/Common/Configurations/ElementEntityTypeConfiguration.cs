using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Entities;

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Common.Configurations;

internal class ElementEntityTypeConfiguration : IEntityTypeConfiguration<ElementEntity>
{
    public void Configure(EntityTypeBuilder<ElementEntity> builder)
    {
        SetTableName(builder);
        SetPrimaryKey(builder);
        ConfigureColumns(builder);
    }

    private void SetTableName(EntityTypeBuilder<ElementEntity> builder)
    {
        builder.ToTable("element");
    }

    private void SetPrimaryKey(EntityTypeBuilder<ElementEntity> builder)
    {
        builder.HasKey(x => x.Id);
    }

    private void ConfigureColumns(EntityTypeBuilder<ElementEntity> builder)
    {
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).HasColumnName("description").HasMaxLength(1000);
        builder.Property(x => x.Value).HasColumnName("value").IsRequired();
        builder.Property(x => x.AdditionalValue).HasColumnName("additional_value");
        builder.Property(x => x.BigValue).HasColumnName("big_value").IsRequired();
        builder.Property(x => x.BigAdditionalValue).HasColumnName("big_additional_value");
        builder.Property(x => x.Price).HasColumnName("price").IsRequired();
        builder.Property(x => x.Discount).HasColumnName("discount");
        builder.Property(x => x.CreationDate).HasColumnName("creation_date").HasColumnType("date").IsRequired();
        builder.Property(x => x.UpdateDate).HasColumnName("update_date").HasColumnType("date");
        builder.Property(x => x.CreationDateTime).HasColumnName("creation_date_time").IsRequired();
        builder.Property(x => x.UpdateDateTime).HasColumnName("update_date_time");
        builder.Property(x => x.IsNew).HasColumnName("is_new").IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("is_active");
    }
}
