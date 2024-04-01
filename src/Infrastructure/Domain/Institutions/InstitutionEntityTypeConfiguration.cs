using App.Domain.Institutions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Domain.Institutions;

public class InstitutionEntityTypeConfiguration : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> builder)
    {
        builder.ToTable("institutions");
        builder.HasKey(i => i.Id);
        builder.Property<InstitutionId>(i => i.Id).HasColumnName("id");

        builder.Property<string>(i => i.BaseUrl).HasColumnName("base_url");
        builder.Property<string?>(i => i.LogoPath).HasColumnName("logo_path");
        builder.Property(i => i.IsEnabled).HasColumnName("is_enabled");
        builder.Property(i => i.IsBeta).HasColumnName("is_beta");

        builder.ComplexProperty<InstitutionName>(i => i.Name, b =>
        {
            b.Property<string>(n => n.PlValue).HasColumnName("name_pl");
            b.Property<string>(n => n.EnValue).HasColumnName("name_en");
        });
    }
}
