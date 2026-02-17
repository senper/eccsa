using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class IndividualEntityTypeConfiguration:
		IEntityTypeConfiguration<Individual>
	{
	  public void Configure(EntityTypeBuilder<Individual> config)
	  {
			config.ToTable("individuals", "membership") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.NameEng)
				.HasColumnName("name_eng")
				.IsRequired()
				.HasMaxLength(200)
				.IsUnicode();


			config.Property(o => o.NameLocal)
				.HasColumnName("name_local")
				.IsRequired()
				.IsUnicode();


			config.Property(o => o.MemberSince)
			.HasColumnName("member_since")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.TaxIdNumber)
				   .HasColumnName("tin")
				   .IsRequired()
				   .HasMaxLength(10);

			config.Property(o => o.Address)
				   .HasColumnName("address")
				   .IsRequired()
				   .HasMaxLength(250)
				   .IsUnicode();

			config.Property(o => o.TradeRegistrationNumber)
					.HasColumnName("trade_registration_no")
					.HasMaxLength(50)
					.IsUnicode();


			config.Property(o => o.LocationId)
					.HasColumnName("location_id")
					.IsRequired();


			config.Property(o => o.SectorId)
				.HasColumnName("sector_id")
				.IsRequired(false);

			config.Property(o => o.TradeSector)
					.HasColumnName("trade_sector")
					.IsRequired();

			config.Property(o => o.BusinessType)
					.HasColumnName("business_type")
					.IsRequired();

			config.Property(o => o.ServiceType)
					.HasColumnName("service_type")
					.IsRequired();

			config.Property(o => o.AgencySpecialization)
					.HasColumnName("agency_specialization")
					.IsRequired();


			config.Property(o => o.AssociatedUserId)
				.HasColumnName("user_id")
				.IsRequired();

			config.Property(o => o.LocationId)
				.HasColumnName("location_id")
				.IsRequired();

            config.Property(o => o.Capital)
                .HasColumnName("capital");


            config.Property(o => o.NumberofEmployees)
                .HasColumnName("no_of_employees");

            config.Property(o => o.CustomFields)
                .HasColumnName("custom_fields")
				.HasColumnType("jsonb")
                .IsRequired(false);

            config.HasMany<BusinessProfile>(o => o.BusinessProfiles)
				.WithOne(s => s.Individual)
				.HasForeignKey(s => s.IndividualId);


			config.HasMany<LegalDocument>(o => o.LegalDocuments)
				.WithOne(s => s.Individual)
				.HasForeignKey(s => s.IndividualId);

			config.HasMany<IndividualBusiness>(o => o.BusinessLicenses)
			.WithOne(s => s.Individual)
			.HasForeignKey(s => s.IndividualId);


			config.HasMany<IndividualManufacturing>(o => o.Manufacturings)
				.WithOne(s => s.Individual)
				.HasForeignKey(s => s.IndividualId);

			config.HasMany<IndividualService>(o => o.Services)
			.WithOne(s => s.Individual)
			.HasForeignKey(s => s.IndividualId);


			config.HasMany<IndividualTrade>(o => o.Trades)
				.WithOne(s => s.Individual)
				.HasForeignKey(s => s.IndividualId);


			config.HasMany<IndividualStaff>(o => o.Employees)
				.WithOne(s => s.Individual)
				.HasForeignKey(s => s.IndividualId);
		}
	}
}