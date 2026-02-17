using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class LegalProfileEntityTypeConfiguration :
		IEntityTypeConfiguration<LegalProfile>
	{
		public void Configure(EntityTypeBuilder<LegalProfile> config)
		{
			config.ToTable("legal_profiles", "services");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.UserId)
			.HasColumnName("user_id")
			.IsRequired();

			config.Property(o => o.Title)
					.HasColumnName("title")
					.HasMaxLength(500)
					.IsRequired()
					.IsUnicode();

			config.Property(o => o.LegalDocumentType)
					.HasColumnName("legal_document_type")
					.HasDefaultValue(LegalDocumentTypes.Other)
					.IsRequired();

			config.Property(o => o.DocumentNumber)
					.HasColumnName("document_number")
					.IsRequired();

			config.Property(o => o.ExpiryDate)
					.HasColumnName("expiry_date");

			config.Property(o => o.FileExtension)
					.HasColumnName("file_extension")
					.HasDefaultValue("")
					.HasMaxLength(10);

			config.Property(o => o.IsLocked)
					.HasColumnName("is_locked")
					.HasDefaultValue(false)
					.IsRequired();

			config.Property(o => o.Status)
					.HasColumnName("status")
					.HasDefaultValue(StatusEnum.Active)
					.IsRequired();
		}
	}
}