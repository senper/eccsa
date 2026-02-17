using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class RenewalHistoryEntityTypeConfiguration:
		IEntityTypeConfiguration<RenewalHistory>
	{
	  public void Configure(EntityTypeBuilder<RenewalHistory> config)
	  {
			config.ToTable("renewal_history", "membership") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.EnrollmentId)
			.HasColumnName("enrollment_id")
			.HasColumnType("uuid")
			.IsRequired();

			config.Property(o => o.ApplicationDate)
			.HasColumnName("application_date")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.RenewalStatus)
			.HasColumnName("renewal_status")
			.IsRequired();




			config.Property(o => o.Fee)
			.HasColumnName("fee")
			.HasColumnType("decimal")
			.IsRequired();


			config.Property(o => o.InvoiceNo)
			.HasColumnName("invoice_no")
			.HasMaxLength(50)
			.IsRequired();


			config.Property(o => o.InvoiceDate)
			.HasColumnName("invoice_date")
			.IsRequired();


			config.Property(o => o.ReceiptNo)
			.HasColumnName("receipt_no")
			.HasMaxLength(50)
			.IsRequired();


			config.Property(o => o.PaymentDate)
			.HasColumnName("payment_date")
			.IsRequired();

			config.Property(o => o.PaymentMethod)
			.HasColumnName("payment_method")
			.IsRequired();

			config.Property(o => o.Remark)
			.HasColumnName("remark")
			.IsRequired();


			config.Property(o => o.RenewalDate)
			.HasColumnName("renewal_date")
			.HasColumnType("date")
			.IsRequired();
		}
	}
}