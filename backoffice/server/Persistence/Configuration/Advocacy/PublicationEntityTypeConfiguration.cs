using ECCSA.BackOffice.Server.Model.Advocacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Advocacy
{
    public class PublicationEntityTypeConfiguration :
		IEntityTypeConfiguration<Publication>
	{
		public void Configure(EntityTypeBuilder<Publication> config)
		{
			config.ToTable("publications", "advocacy");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.Title)
			.HasColumnName("title")
			.HasColumnType("varchar")
			.HasMaxLength(100);

            config.Property(o => o.TitleLocal)
            .HasColumnName("title_local")
            .HasColumnType("varchar")
			.IsUnicode()
			.IsRequired(false)
            .HasMaxLength(100);

            config.Property(o => o.Description)
			.HasColumnName("description")
			.HasMaxLength(800)
			.IsUnicode();

            config.Property(o => o.DescriptionLocal)
            .HasColumnName("description_local")
            .HasMaxLength(800)
			.IsRequired(false)
            .IsUnicode();

            config.Property(o => o.Keywords)
			.HasColumnName("keywords")
			.HasMaxLength(100)
			.IsUnicode();


			config.Property(o => o.Url)
			.HasColumnName("url")
			.HasMaxLength(100)
			.IsUnicode();

			config.Property(o => o.Funding)
			.HasColumnName("funding")
			.HasMaxLength(100)
			.IsUnicode();

            config.Property(o => o.FundingLocal)
            .HasColumnName("funding_local")
            .HasMaxLength(100)
			.IsRequired(false)
            .IsUnicode();

            config.Property(o => o.IsPublic)
			.HasColumnName("is_public");

			config.Property(o => o.Status)
			.HasColumnName("status");

			config.Property(o => o.Remark)
			.HasColumnName("remark")
			.HasMaxLength(100)
			.IsUnicode();

            config.Property(o => o.RemarkLocal)
            .HasColumnName("remark_local")
            .HasMaxLength(100)
			.IsRequired(false)
            .IsUnicode();

            config.Property(o => o.Author)
				.HasColumnName("author")
				.IsRequired()
				.HasMaxLength(100)
				.IsUnicode();

            config.Property(o => o.AuthorLocal)
                .HasColumnName("author_local")
                .IsRequired(false)
                .HasMaxLength(100)
                .IsUnicode();

            config.Property(o => o.PublicationDate)
				   .HasColumnName("publication_date")
				   .HasColumnType("date")
				   .IsRequired();

			

			config.Property(o => o.DigitalCost)
				   .HasColumnName("digital_cost")
				   .IsRequired(false);

			config.Property(o => o.PrintUri)
				  .HasColumnName("print_uri")
				  .HasColumnType("varchar")
				  .HasMaxLength(100)
				  .IsRequired();

			config.Property(o => o.PrintCost)
				   .HasColumnName("print_cost")
				   .IsRequired(false);
			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o => o.CreateStamp)
				.HasColumnName("create_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();

			config.Property(o => o.UpdatedBy)
				.HasColumnName("updated_by")
				.IsRequired();

			config.Property(o => o.UpdateStamp)
				.HasColumnName("update_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();

		}
	}
}