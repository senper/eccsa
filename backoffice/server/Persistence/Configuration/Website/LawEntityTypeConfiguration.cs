using ECCSA.BackOffice.Server.Model.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class LawEntityTypeConfiguration :
			IEntityTypeConfiguration<Law>
	{
		public void Configure(EntityTypeBuilder<Law> config)
		{
			config.ToTable("laws", "website");

			config.HasKey(o => o.Id);
			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.Title)
					.HasColumnName("title")
					.HasMaxLength(100)
					.IsRequired()
					.IsUnicode();
            config.Property(o => o.TitleLocal)
                    .HasColumnName("title_local")
                    .HasMaxLength(100)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.Brief)
                    .HasColumnName("brief")
                    .HasMaxLength(500)
                    .IsRequired()
                    .IsUnicode();
            config.Property(o => o.BriefLocal)
					.HasColumnName("brief_local")
					.HasMaxLength(500)
					.IsRequired()
					.IsUnicode();

			config.Property(o => o.Extension)
					.HasColumnName("extension")
					.HasMaxLength(10)
					.IsRequired();
            config.Property(o => o.ExtensionLocal)
                    .HasColumnName("extension_local")
                    .HasMaxLength(10)
                    .IsRequired();

            config.Property(o => o.LawType)
					.HasColumnName("type")
					.IsRequired();

			config.Property(o => o.Category)
					.HasColumnName("category")
					.IsRequired();

		}
	}
}