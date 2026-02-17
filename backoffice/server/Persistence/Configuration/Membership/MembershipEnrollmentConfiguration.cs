using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class MembershipEnrollmentEntityTypeConfiguration:
		IEntityTypeConfiguration<MembershipEnrollment>
	{
	  public void Configure(EntityTypeBuilder<MembershipEnrollment> config)
	  {
			config.ToTable("membership_enrollment", "membership") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();


			config.Property(o => o.ParentAssociationId)
			.HasColumnName("parent_association_id")
			.IsRequired();


			config.Property(o => o.MemberId)
			.HasColumnName("member_id")
			.IsRequired();

			config.Property(o => o.Code)
			.HasColumnName("code")
			.HasMaxLength(20)
			.IsRequired();


			config.Property(o => o.DateOfEnrollment)
			.HasColumnName("date_of_enrollment")
			.HasColumnType("date")
			.IsRequired();


			config.Property(o => o.MembershipStatus)
			.HasColumnName("membership_status")
			.IsRequired();

			config.Property(o => o.DocumentStatus)
			.HasColumnName("document_status")
			.IsRequired();

			config.Property(o => o.LastRenewalDate)
			.HasColumnName("last_renewal_date")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.LastRenewalId)
			.HasColumnName("last_renewal_id")
			.HasDefaultValue(Guid.Empty)
			.IsRequired();

			config.Property(o => o.RenewalStatus)
			.HasColumnName("renewal_status")
			.IsRequired();

			config.Property(o => o.ExpiryDate)
			.HasColumnName("expiry_date")
			.HasColumnType("date")
			.IsRequired();


			config.Property(o => o.ApplicationFee)
				.HasColumnName("application_fee")
				.HasDefaultValue(0)
				.IsRequired();

			config.Property(o => o.AnnualPayment)
				.HasColumnName("annual_payment")
				.HasDefaultValue(0)
				.IsRequired();

			config.Property(o => o.BankReceiptNo)
				.HasColumnName("bank_recepit_no")
				.HasDefaultValue("")
				.IsRequired();

			config.Property(o => o.InvoiceNo)
				.HasColumnName("invoice_no")
				.HasDefaultValue("")
				.IsRequired();

			config.Property(o => o.Remark)
				.HasColumnName("remark")
				.HasDefaultValue("")
				.IsUnicode()
				.IsRequired();

			config.Property(o => o.IdCard)
				.HasColumnName("idcard_info")
				.HasColumnType("jsonb");

			config.HasMany<RenewalHistory>(o => o.RenewalHistory)
				.WithOne(s => s.MembershipEnrollment)
				.HasForeignKey(s => s.EnrollmentId);




		   

	  }
	}
}