using ECCSA.BackOffice.Server.Model.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Payment
{
	public class InvoiceEntityTypeConfiguration :
		IEntityTypeConfiguration<Invoice>
	{
	  public void Configure(EntityTypeBuilder<Invoice> config)
	  {
			config.ToTable("invoices", "payment") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.SubSystem)
			.HasColumnName("sub_system")
			.IsRequired();


			config.Property(o => o.ReferenceId)
			.HasColumnName("reference_id")
			.IsRequired();


			config.Property(o => o.InvoiceDate)
			.HasColumnName("invoice_date")
			.IsRequired();


			config.Property(o => o.IsCreditService)
			.HasColumnName("is_credit_service")
			.IsRequired();


			config.Property(o => o.CreditDueDays)
			.HasColumnName("credit_due_days")
			.IsRequired();

			config.Property(o => o.InvoiceNo)
				.HasColumnName("invoice_no")
				.HasMaxLength(50)
				.IsRequired();

			config.Property(o => o.CompanyName)
				.HasColumnName("name")
				.HasMaxLength(100)
				.IsRequired();

			config.Property(o => o.TIN)
				.HasColumnName("tin")
				.HasMaxLength(20)
				.IsRequired();

			config.Property(o => o.Address)
				.HasColumnName("address")
				.HasMaxLength(250)
				.IsRequired();

			config.Property(o => o.PaymentReason)
				.HasColumnName("reason")
				.HasMaxLength(250)
				.IsRequired();

			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o => o.CurrencyId)
				.HasColumnName("currency")
				.HasMaxLength(3)
				.IsRequired()
				.HasDefaultValue("ETB");



			config.Property(o => o.BasePrice)
				.HasColumnName("base_price")
				.IsRequired();

		

			config.Property(o => o.Discount)
				.HasColumnName("discount")
				.IsRequired();



			config.Property(o => o.TotalAmount)
				.HasColumnName("total_amount")
				.IsRequired();

			config.Property(o => o.Cancelled)
				.IsRequired()
				.HasColumnName("cancelled");


			config.Property(o => o.CancelledBy)
				.HasColumnName("cancelled_by");

			config.Property(o => o.DateCancelled)
				.HasColumnName("date_cancelled");

			config.Property(o => o.TelePayUrl)
				.HasColumnName("tele_pay_url")
				.HasDefaultValue("");

			config.Property(o => o.OutTradeNo)
				.HasColumnName("out_trade_no")
				.HasDefaultValue("");

			config.HasMany<Settlment>(o => o.Settlments)
				.WithOne(s => s.Invoice)
				.HasForeignKey(s => s.InvoiceId);
		}
	}
}