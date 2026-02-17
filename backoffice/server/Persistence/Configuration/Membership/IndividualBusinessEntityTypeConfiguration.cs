using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class IndividualBusinessEntityTypeConfiguration :
	   IEntityTypeConfiguration<IndividualBusiness>
	{
		public void Configure(EntityTypeBuilder<IndividualBusiness> config)
		{
			config.ToTable("individual_businesses", "membership");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.IndividualId)
			.HasColumnName("individual_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.SerNo)
			.HasColumnName("ser_no")
			.HasMaxLength(10)
			.IsRequired();


			config.Property(o => o.BusinessType)
			.HasColumnName("business_type")
			.HasMaxLength(250)
			.IsRequired()
			.IsUnicode();

			config.Property(o => o.LicenseNo)
			.HasColumnName("license_no")
			.HasMaxLength(250)
			.IsRequired()
			.IsUnicode();

			config.Property(o => o.Capital)
			.HasColumnName("capital")
			.IsRequired();


			config.Property(o => o.IssueDate)
			.HasColumnName("issue_date")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.RenewalDate)
			.HasColumnName("renewal_date")
			.HasColumnType("date")
			.IsRequired();



		}
	}
}