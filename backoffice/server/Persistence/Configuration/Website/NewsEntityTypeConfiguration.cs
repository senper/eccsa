using ECCSA.BackOffice.Server.Model.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class NewsEntityTypeConfiguration :
			IEntityTypeConfiguration<News>
	{
		public void Configure(EntityTypeBuilder<News> config)
		{
			config.ToTable("news", "website");

			config.HasKey(o=> o.Id);
			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.Title)
					.HasColumnName("title")
					.HasMaxLength(200)
					.IsRequired()
					.IsUnicode();

            config.Property(o => o.TitleLocal)
                    .HasColumnName("title_local")
                    .HasMaxLength(200)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.Brief)
                    .HasColumnName("brief")
                    .HasMaxLength(1000)
                    .IsRequired()
                    .IsUnicode();
            config.Property(o => o.BriefLocal)
					.HasColumnName("brief_local")
					.HasMaxLength(1000)
					.IsRequired()
					.IsUnicode();

			config.Property(o => o.Full)
					.HasColumnName("full")
					.IsUnicode();
            config.Property(o => o.FullLocal)
                    .HasColumnName("full_local")
                    .IsUnicode();
            config.Property(o => o.OrderNo)
					.HasColumnName("order_no")
					.HasMaxLength(10)
					.IsRequired();

			config.Property(o => o.Author)
					.HasColumnName("author")
					.HasMaxLength(100)
					.IsRequired();
            config.Property(o => o.AuthorLocal)
                    .HasColumnName("author_local")
                    .HasMaxLength(100)
                    .IsRequired();

            config.Property(o => o.NewsDate)
					.HasColumnName("news_date");

			config.Property(o=> o.Extension)
					.HasColumnName("extension")
					.HasDefaultValue("")
					.HasMaxLength(10)
					.IsRequired();

			config.Property(o => o.ExtensionHome)
					.HasColumnName("extension_home")
					.HasMaxLength(10)
					.HasDefaultValue("")
					.IsRequired();
		}
	}
}