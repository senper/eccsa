using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class LegalDocumentEntityTypeConfiguration:
		IEntityTypeConfiguration<LegalDocument>
	{
	  public void Configure(EntityTypeBuilder<LegalDocument> config)
	  {
			config.ToTable("legal_documents", "membership") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.IndividualId)
			.HasColumnName("individual_id")
			.HasColumnType("uuid")
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
			config.Property(o => o.IntResponse)
					.HasColumnName("int_response")
					.IsRequired(false);


			config.Property(o => o.Extension)
				.HasColumnName("extension")
				.IsRequired()
				.HasDefaultValue("")
				.HasMaxLength(10);

			config.Property(o => o.Remark)
				.HasColumnName("remark")
				.IsRequired()
				.HasDefaultValue("")
				.HasMaxLength(250);




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












		}
	}
}