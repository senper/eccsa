using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class ExportAgreementEntityTypeConfiguration :
		IEntityTypeConfiguration<ExportAgreement>
	{
		public void Configure(EntityTypeBuilder<ExportAgreement> config)
		{
			config.ToTable("export_agreements", "services");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.UserId)
			.HasColumnName("user_id")
			.IsRequired();

			config.Property(o => o.AgreementNo)
					.HasColumnName("agreement_no")
					.HasMaxLength(50);

			config.Property(o => o.LicenseNo)
					.HasColumnName("license_no")
					.HasMaxLength(50);

			config.Property(o => o.Consignee)
					.HasColumnName("consignee")
					.HasMaxLength(255);

			config.Property(o => o.Address)
					.HasColumnName("address")
					.HasMaxLength(255);

			config.Property(o => o.TermOfDelivery)
					.HasColumnName("term_of_delivery")
					.HasMaxLength(255);

			config.Property(o => o.Country)
					.HasColumnName("country")
					.HasMaxLength(50);

			config.Property(o => o.City)
					.HasColumnName("city")
					.HasMaxLength(50);

			config.Property(o => o.TypeOfCarriage)
					.HasColumnName("type_of_carriage")
					.HasMaxLength(50);

			config.Property(o => o.CountyOfDestination)
					.HasColumnName("county_of_destination")
					.HasMaxLength(50);

			config.Property(o => o.PortOfDestination)
					.HasColumnName("port_of_destination")
					.HasMaxLength(50);

			config.Property(o => o.PortOfDeparture)
					.HasColumnName("port_of_departure")
					.HasMaxLength(50);

			config.Property(o => o.NameOfCarrier)
					.HasColumnName("name_of_carrier")
					.HasMaxLength(50);

			config.Property(o => o.IsLocked)
					.HasColumnName("is_locked")
					.HasDefaultValue(false)
					.IsRequired();

			config.Property(o => o.Status)
					.HasColumnName("status")
					.HasDefaultValue(StatusEnum.Active)
					.IsRequired();

			config.HasMany(o => o.Commodities)
				  .WithOne(s => s.Agreement)
				 .HasForeignKey(s => s.AgreementId);
		}
	}

	public class AgreementCommodityEntityTypeConfiguration :
		IEntityTypeConfiguration<AgreementCommodity>
	{
		public void Configure(EntityTypeBuilder<AgreementCommodity> config)
		{
			config.ToTable("agreement_commodities", "services");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.AgreementId)
			.HasColumnName("agreement_id")
			.IsRequired();

			config.Property(o => o.HSCode)
					.HasColumnName("hs_code")
					.HasMaxLength(50);

			config.Property(o => o.Name)
					.HasColumnName("name")
					.HasMaxLength(255);
		}
	}
}