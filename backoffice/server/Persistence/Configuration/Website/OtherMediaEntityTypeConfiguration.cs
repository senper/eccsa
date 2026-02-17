using ECCSA.BackOffice.Server.Model.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class OtherMediaEntityTypeConfiguration :
			IEntityTypeConfiguration<OtherMedia>
	{
		public void Configure(EntityTypeBuilder<OtherMedia> config)
		{
			config.ToTable("other_media", "website");

			config.HasKey(o => o.Id);
			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.MediaTypeId)
				.HasColumnName("media_type_id")
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

            config.Property(o => o.Content)
					.HasColumnName("content")
					.IsRequired()
					.IsUnicode();
            config.Property(o => o.ContentLocal)
                    .HasColumnName("content_local")
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.AuthorName)
					.HasColumnName("author_name")
					.HasMaxLength(50)
					.IsRequired();
            config.Property(o => o.AuthorNameLocal)
                    .HasColumnName("author_name_local")
                    .HasMaxLength(50)
					.IsUnicode()
                    .IsRequired();
            config.Property(o => o.Author)
					.HasColumnName("author")
					.IsRequired();

			config.Property(o => o.DateCreated)
					.HasColumnName("date_created")
					.IsRequired();
			config.Property(o => o.Views)
					.HasColumnName("views")
					.IsRequired();
			config.Property(o => o.Likes)
					.HasColumnName("likes")
					.IsRequired();
			config.Property(o => o.Dislikes)
					.HasColumnName("dislikes")
					.IsRequired();
			config.Property(o => o.Shares)
					.HasColumnName("shares")
					.IsRequired();
			config.Property(o => o.Comments)
					.HasColumnName("comments")
					.HasColumnType("jsonb")
					.IsRequired();

		}
	}
}