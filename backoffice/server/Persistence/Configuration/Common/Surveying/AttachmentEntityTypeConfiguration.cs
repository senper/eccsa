using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class AttachmentEntityTypeConfiguration :
		IEntityTypeConfiguration<Attachment>
	{
		public void Configure(EntityTypeBuilder<Attachment> config)
		{
			config.ToTable("attachments", "surveying");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
					.HasColumnName("id")
					.ValueGeneratedOnAdd()
					.IsRequired();

			config.Property(o => o.SurveyId)
				.HasColumnName("survey_id")
				.IsRequired();
			config.Property(o => o.Title)
					.HasColumnName("title")
					.HasMaxLength(100)
					.IsUnicode()
					.IsRequired();

			config.Property(o => o.Description)
					.HasColumnName("description")
					.IsUnicode()
					.IsRequired();

			config.Property(o => o.Extension)
					.HasColumnName("extension")
					.HasMaxLength(50)
					.HasDefaultValue("")
					.IsRequired();

			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o => o.CreateStamp)
				.HasColumnName("create_stamp")
				.HasDefaultValue(DateTime.UtcNow)
				.IsRequired();

			config.Property(o => o.UpdatedBy)
				.HasColumnName("updated_by")
				.IsRequired();

			config.Property(o => o.UpdateStamp)
				.HasColumnName("update_stamp")
				.HasDefaultValue(DateTime.UtcNow)
				.IsRequired();

		

		}
	}
}