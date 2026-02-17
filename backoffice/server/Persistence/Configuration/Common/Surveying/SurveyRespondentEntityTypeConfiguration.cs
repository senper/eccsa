using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class SurveyRespondentEntityTypeConfiguration :
		IEntityTypeConfiguration<SurveyRespondent>
	{
		public void Configure(EntityTypeBuilder<SurveyRespondent> config)
		{
			config.ToTable("survey_respondents", "surveying");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
					.HasColumnName("id")
					.ValueGeneratedOnAdd()
					.IsRequired();

			config.Property(o => o.SurveyId)
				.HasColumnName("survey_id")
				.IsRequired();

			config.Property(o => o.UserId)
				.HasColumnName("respondent_user_id")
				.IsRequired();

			config.Property(o => o.StartDate)
				.HasColumnName("start_date")
				.IsRequired();
			config.Property(o => o.EndDate)
				.HasColumnName("end_date")
				.IsRequired();

			config.Property(o => o.Remark)
				.HasColumnName("remark")
				.IsUnicode();

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



			config.HasMany<SurveyResponse>(o => o.Responses)
				.WithOne(s => s.Respondent)
				.HasForeignKey(s => s.SurveyRespondentId);

		}
	}

}