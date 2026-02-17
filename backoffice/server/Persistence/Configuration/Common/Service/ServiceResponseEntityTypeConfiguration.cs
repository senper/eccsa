using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class ServiceResponseEntityTypeConfiguration :
	IEntityTypeConfiguration<ServiceRequirementResponse>
	{
		public void Configure(EntityTypeBuilder<ServiceRequirementResponse> config)
		{
			config.ToTable("service_responses", "services");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd()
			.IsRequired();


			config.Property(o => o.ServiceRequestId)
					.HasColumnName("service_request_id")
					.IsRequired();
			config.Property(o => o.RequirementId)
					.HasColumnName("requirement_id")
					.IsRequired();

			config.Property(o => o.TextResponse)
					.HasColumnName("text_response")
					.IsUnicode()
					.IsRequired(false);
			config.Property(o => o.DateResponse)
					.HasColumnName("date_response")
					.IsRequired(false);
			config.Property(o => o.DecimalResponse)
					.HasColumnName("decimal_response")
					.IsRequired(false);

			config.Property(o => o.TabularResponse)
					.HasColumnName("json_response")
					.HasColumnType("jsonb")
					.IsRequired(false);

			config.Property(o => o.IntResponse)
					.HasColumnName("int_response")
					.IsRequired(false);

			config.Property(o => o.PrintReceipt)
					.HasColumnName("print_receipt")
					.IsRequired()
					.HasDefaultValue(false);

			config.Property(o => o.Remark)
					.HasColumnName("remark")
					.IsRequired()
					.IsUnicode();

			config.Property(o => o.DateSubmitted)
					.HasColumnName("date_submitted")
					.IsRequired(false);

			config.Property(o => o.ReviewedBy)
					.HasColumnName("reviewed_by")
					.IsRequired(false);

			config.Property(o => o.DateReviewed)
				  .HasColumnName("date_reviewed")
				  .IsRequired(false);

			config.Property(o => o.ReviewStatus)
					.HasColumnName("review_status")
					.HasDefaultValue(DocumentReviewStatusEnum.NotSet)
					.IsRequired();


			config.Property(o => o.LookupValueId)
				.HasColumnName("lookup_id")
				.IsRequired(false);

			config.Property(o => o.InvoiceNo)
					.HasColumnName("invoice_no")
					.HasMaxLength(50)
					.IsRequired(false);

			config.Property(o => o.PaymentMethod)
					.HasColumnName("payment_method")
					.HasDefaultValue(PaymentMethodEnum.NotSet)
					.IsRequired();

			config.Property(o => o.InvoiceAmount)
					.HasColumnName("invoice_amount")
					.IsRequired(false);

			config.Property(o => o.InvoiceDate)
				  .HasColumnName("invoice_date")
				  .HasColumnType("date")
				  .IsRequired(false);

			config.Property(o => o.ReceiptNo)
					.HasColumnName("receipt_no")
					.HasMaxLength(50)
					.IsRequired(false);

			config.Property(o => o.DatePaid)
				  .HasColumnName("date_paid")
				  .HasColumnType("date")
				  .IsRequired(false);

			config.Property(o => o.AmountPaid)
					.HasColumnName("amount_paid")
					.IsRequired(false);

			config.Property(o => o.PaymentStatus)
					.HasColumnName("receipt_status")
					.HasDefaultValue(PaymentStatusEnum.NotSet)
					.IsRequired();

		}
	}


}