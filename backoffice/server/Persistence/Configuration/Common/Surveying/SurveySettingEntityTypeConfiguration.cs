using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class SurveySettingEntityTypeConfiguration :
		IEntityTypeConfiguration<SurveySetting>
	{
		public void Configure(EntityTypeBuilder<SurveySetting> config)
		{
			config.ToTable("survey_settings","surveying");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
					.HasColumnName("id")
					.ValueGeneratedOnAdd()
					.IsRequired();

			config.Property(o => o.Title)
					.HasColumnName("title")
					.IsRequired()
					.HasMaxLength(200)
					.IsUnicode();


			config.Property(o => o.Description)
				.HasColumnName("description")
				.IsRequired()
				.IsUnicode();

			config.Property(o => o.ResponsibleUnit)
				.HasColumnName("responsible_unit")
				.IsRequired();


			config.Property(o => o.OpenDays)
				.HasColumnName("open_days")
				.IsRequired();

			config.Property(o => o.PublishThrough)
				.HasColumnName("publish_through")
				.IsRequired();

			config.Property(o => o.Status)
				.HasColumnName("status")
				.IsRequired();

			config.Property(o => o.IsPublic)
				.HasColumnName("is_public")
				.IsRequired();

			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o=> o.CreateStamp)
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

			config.HasMany<Survey>(o => o.Surveys)
				.WithOne(s => s.Setting)
				.HasForeignKey(s => s.SettingId);

			config.HasMany<SurveyQuestion>(o => o.Questions)
				.WithOne(s => s.Setting)
				.HasForeignKey(s => s.SurveySettingId);

			config.HasMany<SurveyChoice>(o => o.Choices)
				.WithOne(s => s.SurveySetting)
				.HasForeignKey(s => s.SurveySettingId);

		}
	}

}