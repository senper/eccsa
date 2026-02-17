using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class SelectedChoiceEntityTypeConfiguration :
		IEntityTypeConfiguration<SelectedChoice>
	{
		public void Configure(EntityTypeBuilder<SelectedChoice> config)
		{
			config.ToTable("selected_choices", "surveying");


			config.HasKey(o => new { o.ResponseId, o.ChoiceId });



			config.Property(o => o.ResponseId)
				.HasColumnName("response_id")
				.IsRequired();

			config.Property(o => o.ChoiceId)
				.HasColumnName("choice_id")
				.IsRequired();

			config.Property(o => o.Rank)
				.HasColumnName("rank")
				.HasDefaultValue(0)
				.IsRequired();

		}
	}

}