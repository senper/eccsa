using ECCSA.BackOffice.Server.Model.Arbitration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Arbitration
{
	public class ArbitrationDocumentEntityTypeConfiguration :
		IEntityTypeConfiguration<ArbitrationDocument>
	{
		public void Configure(EntityTypeBuilder<ArbitrationDocument> config)
		{
			config.ToTable("arbitration_documents", "arbitration");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd()
			.IsRequired();


			config.Property(o => o.ServiceId)
				.HasColumnName("service_id")
				.IsRequired();


			config.Property(o => o.OrderNo)
					.HasColumnName("order_no")
					.HasMaxLength(20);

			config.Property(o => o.DocumentTypeId)
				.HasColumnName("document_type_id")
				.IsRequired();

			config.Property(o => o.Name)
					.HasColumnName("name")
					.IsUnicode();

			config.Property(o => o.Content)
					.HasColumnName("content")
					.IsUnicode();

			config.Property(o => o.DateSubmitted)
				.HasColumnName("date_submitted");

			config.Property(o => o.ReviewedBy)
				.HasColumnName("reviewd_by");


			config.Property(o => o.Remark)
				.HasColumnName("remark");

			config.Property(o => o.ReviewStatus)
				.HasColumnName("review_status");

			config.Property(o => o.DateReviewed)
				.HasColumnName("date_reviewed");
		}
	}
}