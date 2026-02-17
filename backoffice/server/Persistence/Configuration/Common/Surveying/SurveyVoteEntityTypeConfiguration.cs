using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class SurveyVoteEntityTypeConfiguration :
		IEntityTypeConfiguration<SurveyVote>
	{
		public void Configure(EntityTypeBuilder<SurveyVote> config)
		{
			config.ToTable("survey_votes", "surveying");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
					.HasColumnName("id")
					.ValueGeneratedOnAdd()
					.IsRequired();

			config.Property(o => o.SurveyId)
				.HasColumnName("survey_id")
				.IsRequired();

			config.Property(o => o.QuestionId)
				.HasColumnName("question_id")
				.IsRequired();

			config.Property(o => o.ChoiceId)
				.HasColumnName("choice_id")
				.IsRequired();

			config.Property(o => o.Votes)
				.HasColumnName("votes")
				.IsRequired();


		}
	}
}