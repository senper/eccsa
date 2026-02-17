using ECCSA.BackOffice.Server.Model.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class TranslationEntityTypeConfiguration :
            IEntityTypeConfiguration<Translation>
    {
        public void Configure(EntityTypeBuilder<Translation> config)
        {
            config.ToTable("translations", "website");

            config.HasKey(o => o.Key);
            config.Property(o => o.Key)
                .HasColumnName("key")
                .IsRequired();

            config.Property(o => o.Default)
                    .HasColumnName("default")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.Local)
                    .HasColumnName("local")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            
        }
    }
}