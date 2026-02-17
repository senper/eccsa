using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class SurveyAudienceEntityTypeConfiguration :
		IEntityTypeConfiguration<SurveyAudience>
	{
		public void Configure(EntityTypeBuilder<SurveyAudience> config)
		{
			config.ToTable("survey_audiences", "surveying");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
					.HasColumnName("id")
					.ValueGeneratedOnAdd()
					.IsRequired();

			config.Property(o => o.SurveyId)
				.HasColumnName("survey_id")
				.IsRequired();

			config.Property(o => o.UserId)
				.HasColumnName("user_id")
				.IsRequired();

			config.Property(o => o.Email)
				.HasColumnName("email")
				.IsRequired();

			config.Property(o => o.MobileNo)
					.HasColumnName("mobile_no")
					.IsRequired();
			


		}
	}

}