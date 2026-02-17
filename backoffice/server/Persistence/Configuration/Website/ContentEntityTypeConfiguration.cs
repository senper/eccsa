using ECCSA.BackOffice.Server.Model.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class ContentEntityTypeConfiguration :
		   IEntityTypeConfiguration<SearchResult>
	{
		public void Configure(EntityTypeBuilder<SearchResult> config)
		{
			config.ToTable("contents", "website");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

		
			config.Property(o => o.Title)
					.HasColumnName("title")
					.HasMaxLength(200)
					.IsUnicode();
			config.Property(o => o.Brief)
					.HasColumnName("description")
					.HasMaxLength(500)
					.IsUnicode();
			config.Property(o => o.ContentType)
					.HasColumnName("content_type")
					.IsRequired();



		}
	}
}