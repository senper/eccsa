using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class SurveyChoiceEntityTypeConfiguration :
		IEntityTypeConfiguration<SurveyChoice>
	{
		public void Configure(EntityTypeBuilder<SurveyChoice> config)
		{
			config.ToTable("survey_choices", "surveying");


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

			config.Property(o => o.QuestionId)
					.HasColumnName("question_id")
					.IsRequired();

			config.Property(o => o.ChoiceText)
				.HasColumnName("choice_text")
				.HasMaxLength(100)
				.IsUnicode()
				.IsRequired();

			config.HasMany<SelectedChoice>(o => o.Responses)
				.WithOne(s => s.Choice)
				.HasForeignKey(s => s.ChoiceId);

			config.HasMany<SurveyVote>(o => o.Votes)
				.WithOne(s => s.Choice)
				.HasForeignKey(s => s.ChoiceId);

		}
	}
}