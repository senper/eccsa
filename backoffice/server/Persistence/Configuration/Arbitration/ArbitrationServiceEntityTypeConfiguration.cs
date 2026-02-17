using ECCSA.BackOffice.Server.Model.Arbitration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Arbitration
{
	public class ArbitrationServiceEntityTypeConfiguration :
		IEntityTypeConfiguration<ArbitrationService>
	{
		public void Configure(EntityTypeBuilder<ArbitrationService> config)
		{
			config.ToTable("arbitration_services", "arbitration");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd()
			.IsRequired();

			config.Property(o => o.FullName)
					.HasColumnName("full_name")
					.IsUnicode()
					.IsRequired();

			config.Property(o => o.MobileNo)
					.HasColumnName("mobile_no")
				.HasColumnType("varchar")
				.HasMaxLength(50)
					.IsRequired();

			config.Property(o => o.EmailAddress)
					.HasColumnName("email_address")
					.IsUnicode();

			config.Property(o => o.TIN)
					.HasColumnName("tin")
				.HasColumnType("varchar")
				.HasMaxLength(255)
					.IsUnicode()
					.IsRequired();

			config.Property(o => o.ServiceTypeId)
					.HasColumnName("service_type_id")
					.IsRequired();

			config.Property(o => o.RequestDate)
					.HasColumnName("request_date")
					.IsRequired();

			config.Property(o => o.AssociationId)
					.HasColumnName("association_id")
					.IsRequired();

			config.Property(o => o.PaymentMethod)
					.HasColumnName("payment_method")
					.IsRequired();

			config.Property(o => o.RequestStatus)
					.HasColumnName("request_status")
					.IsRequired();

			config.Property(o => o.InvoiceNo)
				.HasColumnName("invoice_no")
				.HasColumnType("varchar")
				.HasMaxLength(20);


			config.Property(o => o.ReceiptNo)
					.HasColumnName("receipt_no")
					.HasMaxLength(50)
					.IsUnicode();


			config.Property(o => o.Description)
					.HasColumnName("description")
					.IsUnicode();


			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o=> o.CreateStamp)
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

			config.HasMany<ArbitrationDocument>(o => o.Documents)
				.WithOne(s => s.ArbitrationService)
				.HasForeignKey(s => s.ServiceId);

			config.HasMany<ArbitrationFollowup>(o => o.Followups)
				.WithOne(s => s.ArbitrationService)
				.HasForeignKey(s => s.ServiceId);


			config.HasMany<ArbitrationRelatedParty>(o => o.RelatedParties)
				.WithOne(s => s.ArbitrationService)
				.HasForeignKey(s => s.ServiceId);



		}
	}
}