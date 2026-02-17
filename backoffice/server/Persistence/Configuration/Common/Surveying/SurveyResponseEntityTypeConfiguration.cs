using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class SurveyResponseEntityTypeConfiguration :
		IEntityTypeConfiguration<SurveyResponse>
	{
		public void Configure(EntityTypeBuilder<SurveyResponse> config)
		{
			config.ToTable("survey_responses", "surveying");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
					.HasColumnName("id")
					.ValueGeneratedOnAdd()
					.IsRequired();

			config.Property(o => o.QuestionId)
				.HasColumnName("question_id")
				.IsRequired();

			config.Property(o => o.RespondentName)
				.HasColumnName("respondent_name");

			config.Property(o => o.SurveyId)
				.HasColumnName("survey_id")
				.IsRequired();

			config.Property(o => o.SurveyRespondentId)
				.HasColumnName("respondent_id");

			config.Property(o => o.NumericResponse)
				.HasColumnName("numeric_response")
				.HasDefaultValue(decimal.Zero)
				.IsRequired();
			config.Property(o => o.TextualResponse)
				.HasColumnName("textual_response")
				.IsRequired();


			config.HasMany<SelectedChoice>(o => o.Choices)
				.WithOne(s => s.Response)
				.HasForeignKey(s => s.ResponseId);

		}
	}

}