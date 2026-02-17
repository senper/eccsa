using ECCSA.BackOffice.Server.Model.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Payment
{
    public class SettlementEntityTypeConfiguration :
		IEntityTypeConfiguration<Settlment>
	{
		public void Configure(EntityTypeBuilder<Settlment> config)
		{
			config.ToTable("settlments", "payment");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.InvoiceId)
			.HasColumnName("invoice_id")
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


			config.Property(o => o.AmountPaid)
			.HasColumnName("amount_paid")
			.IsRequired();

			config.Property(o => o.AmountConfirmed)
			.HasColumnName("amount_confirmed")
			.HasDefaultValue(0)
			.IsRequired();

			config.Property(o => o.Status)
					.HasColumnName("receipt_status")
					.HasDefaultValue(Model.Common.PaymentStatusEnum.NotSet)
					.IsRequired();

			config.Property(o => o.Extension)
				.HasColumnName("extension")
				.IsRequired();

			config.Property(o => o.Remark)
				.HasColumnName("remark")
				.IsRequired();

			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o => o.CreatedOn)
				.HasColumnName("created_on")
				.IsRequired();




		}
	}
}