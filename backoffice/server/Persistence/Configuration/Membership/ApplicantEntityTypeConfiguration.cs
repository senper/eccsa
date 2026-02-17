using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class ApplicantEntityTypeConfiguration :
		IEntityTypeConfiguration<Applicant>
	{
		public void Configure(EntityTypeBuilder<Applicant> config)
		{
			config.ToTable("applicants", "membership");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd()
			.IsRequired();


			config.Property(o => o.NameEng)
			.HasColumnName("name_eng")

			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();


			config.Property(o => o.NameLocal)
			.HasColumnName("name_local")
			.HasMaxLength(400)
			.IsRequired()

			.IsUnicode();
			config.Property(o => o.Extension)
				   .HasColumnName("extension")
				   .HasDefaultValue(string.Empty)
				   .HasMaxLength(10)
				   .IsRequired();

			config.Property(o => o.ApplicationDate)
			.HasColumnName("application_date")
			.HasColumnType("date")
			.IsRequired();


			config.Property(o => o.TrackingNo)
				   .HasColumnName("tracing_no")
				   .IsRequired()
				   .HasMaxLength(20);

			config.Property(o => o.ApplicationStatus)
			.HasColumnName("application_status")
			.IsRequired();

			config.Property(o => o.Remark)
				.HasColumnName("remark")
				.HasMaxLength(200)
				.IsRequired();

			config.Property(o => o.ApplicationFee)
				.HasColumnName("application_fee")
				.HasDefaultValue(0)
				.IsRequired();

			config.Property(o => o.IndividualId)
				.HasColumnName("individual_id")
				.IsRequired(false);

			config.Property(o => o.ApplicationType)
				.HasColumnName("application_type")
				.IsRequired();

			config.Property(o => o.ReceiptNo)
				.HasColumnName("recepit_no")
				.HasDefaultValue("")
				.IsRequired();

			config.Property(o => o.PaymentStatus)
					.HasColumnName("receipt_status")
					.HasDefaultValue(PaymentStatusEnum.NotSet)
					.IsRequired();

			config.Property(o => o.InvoiceNo)
				.HasColumnName("invoice_no")
				.HasDefaultValue("")
				.IsRequired();

			config.Property(o => o.AssociatedUserId)
				.HasColumnName("user_id")
				.IsRequired();


			config.Property(o => o.AssociationId)
				.HasColumnName("association_id")
				.IsRequired();

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

			config.Property(o => o.PaymentMethod)
					.HasColumnName("payment_method")
					.HasDefaultValue(PaymentMethodEnum.NotSet)
					.IsRequired();

			config.Property(o => o.Signature)
				   .HasColumnName("signature")
				   .HasDefaultValue(string.Empty)
				   .HasMaxLength(500)
				   .IsRequired();

			config.Property(o => o.OperatorName)
				   .HasMaxLength(50)
				   .HasDefaultValue(string.Empty)
				   .HasColumnName("operator_name");

			config.Property(o => o.ApproverName)
					.HasMaxLength(50)
					.HasDefaultValue(string.Empty)
					.HasColumnName("approver_name");
			config.Property(o => o.PrintReceipt)
				  .HasColumnName("print_receipt")
				  .HasDefaultValue(false)
				  .IsRequired();

			config.Property(o => o.TIN)
					.HasColumnName("tin")
					.IsRequired()
					.HasMaxLength(20);


			

			config.Property(o => o.EmailAddress)
					.HasColumnName("email")
					.IsRequired()
					.HasMaxLength(200);

			config.Property(o => o.Address)
					.HasColumnName("address")
					.IsRequired()
					.HasMaxLength(200);

			config.Property(o => o.MobileNo)
					.HasColumnName("mobile")
					.IsRequired()
					.HasMaxLength(20);

			config.HasMany<ApplicantDocument>(o => o.Responses)
				.WithOne(s => s.Applicant)
				.HasForeignKey(s => s.ApplicantId);
		}
	}
}