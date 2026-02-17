using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class BusinessInformationEntityTypeConfiguration :
	   IEntityTypeConfiguration<BusinessInformation>
	{
		public void Configure(EntityTypeBuilder<BusinessInformation> config)
		{
			config.ToTable("business_information", "membership");
			config.HasKey(o => o.Id);
			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.Title)
			.HasColumnName("title")
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();

            config.Property(o => o.TitleLocal)
            .HasColumnName("title_local")
            .IsRequired(false)
            .HasMaxLength(200)
            .IsUnicode();

            config.Property(o => o.Link)
			.HasColumnName("link")
			.HasMaxLength(500)
			.IsUnicode();

            config.Property(o => o.Extension)
           .HasColumnName("extension")
           .HasMaxLength(10);

            config.Property(o => o.EndDate)
			.HasColumnName("end_date")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.Description)
			.HasColumnName("description")
			.HasMaxLength(1000)
			.IsRequired()
			.IsUnicode();

            config.Property(o => o.DescriptionLocal)
            .HasColumnName("description_local")
            .HasMaxLength(1000)
            .IsRequired(false)
            .IsUnicode();
        }
	}

}