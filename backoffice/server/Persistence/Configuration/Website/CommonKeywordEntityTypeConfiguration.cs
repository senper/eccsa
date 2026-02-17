using ECCSA.BackOffice.Server.Model.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class CommonKeywordEntityTypeConfiguration :
		   IEntityTypeConfiguration<CommonKeyword>
	{
		public void Configure(EntityTypeBuilder<CommonKeyword> config)
		{
			config.ToTable("common_keywords", "website");

			config.HasKey(o => o.ContentType);

			config.Property(o => o.ContentType)
					.HasColumnName("content_type")
					.IsRequired();


			config.Property(o => o.Keywords)
					.HasColumnName("keywords")
					.HasColumnType("jsonb");
			



		}
	}
}