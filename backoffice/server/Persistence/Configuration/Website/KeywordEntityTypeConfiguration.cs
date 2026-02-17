using ECCSA.BackOffice.Server.Model.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class KeywordEntityTypeConfiguration :
			IEntityTypeConfiguration<Keyword>
	{
		public void Configure(EntityTypeBuilder<Keyword> config)
		{
			config.ToTable("keywords", "website");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
				.HasColumnName("keyword_id")
				.IsRequired();

			config.Property(o => o.ContentId)
				.HasColumnName("content_id")
				.IsRequired();

			config.Property(o => o.DetailId)
				.HasColumnName("detail_id")
				.IsRequired();
			config.Property(o => o.Word)
					.HasColumnName("word")
					.HasMaxLength(50)
					.IsUnicode();
			config.Property(o => o.Normalized)
					.HasColumnName("normalized")
					.HasMaxLength(50)
					.IsUnicode();
			config.Property(o => o.ContentType)
					.HasColumnName("content_type")
					.IsRequired();



		}
	}
}