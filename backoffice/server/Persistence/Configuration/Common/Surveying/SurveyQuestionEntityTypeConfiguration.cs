using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class SurveyQuestionEntityTypeConfiguration :
		IEntityTypeConfiguration<SurveyQuestion>
	{
		public void Configure(EntityTypeBuilder<SurveyQuestion> config)
		{
			config.ToTable("survey_questions", "surveying");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
					.HasColumnName("id")
					.ValueGeneratedOnAdd()
					.IsRequired();

			config.Property(o => o.SurveySettingId)
				.HasColumnName("setting_id")
				.IsRequired();

			config.Property(o => o.OrderNo)
				.HasColumnName("order_no")
				.HasMaxLength(20)
				.IsRequired();

			config.Property(o => o.Question)
					.HasColumnName("question")
					.IsRequired()
					.HasMaxLength(200)
					.IsUnicode();
			config.Property(o => o.QuestionType)
				.HasColumnName("question_type")
				.IsRequired();

			config.Property(o => o.MinRange)
				.HasColumnName("min_range");

			config.Property(o => o.MaxRange)
				.HasColumnName("max_range");

			config.Property(o => o.Cascaded)
				.HasColumnName("cascade")
				.IsRequired();

			config.Property(o => o.ParentChoiceId)
				.HasColumnName("parent_choice_id");

			config.HasMany<SurveyChoice>(o => o.Choices)
				.WithOne(s => s.Question)
				.HasForeignKey(s => s.QuestionId);

			config.HasMany<SurveyVote>(o => o.Votes)
				.WithOne(s => s.Question)
				.HasForeignKey(s => s.QuestionId);
		}
	}

}