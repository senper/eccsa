using ECCSA.BackOffice.Server.Model.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class AdvertizementEntityTypeConfiguration :
			IEntityTypeConfiguration<Advertizement>
	{
		public void Configure(EntityTypeBuilder<Advertizement> config)
		{
			config.ToTable("advertizements", "website");


			config.HasKey(p=> p.Id);
			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.FullName)
				.HasColumnName("full_name")
				.HasMaxLength(100)
				.IsUnicode()
				.IsRequired();

			config.Property(o => o.CompanyName)
				.HasColumnName("company_name")
				.HasMaxLength(100)
				.IsUnicode()
				.IsRequired();

			config.Property(o => o.Email)
				.HasColumnName("email")
				.HasMaxLength(100)
				.IsUnicode()
				.IsRequired();

			config.Property(o => o.PhoneNumber)
				.HasColumnName("phone_number")
				.HasMaxLength(50)
				.IsUnicode()
				.IsRequired();

		

			config.Property(o => o.Section)
					.HasColumnName("section")
					.IsRequired();

			config.Property(o => o.StartDate)
					.HasColumnName("start_date")
					.HasColumnType("date")
					.IsRequired();
			config.Property(o => o.EndDate)
					.HasColumnName("end_date")
					.HasColumnType("date")
					.IsRequired();

			config.Property(o => o.Extension)
				.HasColumnName("extension")
				.HasMaxLength(10)
				.IsRequired();


		}
	}



	public class PartnerEntityTypeConfiguration :
			IEntityTypeConfiguration<Partner>
	{
		public void Configure(EntityTypeBuilder<Partner> config)
		{
			config.ToTable("partners", "website");



			config.Property(o => o.Id)
				.HasColumnName("partner_id")
				.IsRequired();

			config.Property(o => o.FullName)
				.HasColumnName("full_name")
				.HasMaxLength(100)
				.IsUnicode()
				.IsRequired();

			config.Property(o => o.CompanyName)
				.HasColumnName("company_name")
				.HasMaxLength(100)
				.IsUnicode()
				.IsRequired();

			config.Property(o => o.Email)
				.HasColumnName("email")
				.HasMaxLength(100)
				.IsUnicode()
				.IsRequired();

			config.Property(o => o.PhoneNumber)
				.HasColumnName("phone_number")
				.HasMaxLength(50)
				.IsUnicode()
				.IsRequired();

			config.Property(o => o.StartDate)
					.HasColumnName("start_date")
					.HasColumnType("date")
					.IsRequired();
			config.Property(o => o.EndDate)
					.HasColumnName("end_date")
					.HasColumnType("date")
					.IsRequired();



		}
	}
}