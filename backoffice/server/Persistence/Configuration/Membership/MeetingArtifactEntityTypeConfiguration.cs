using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class MeetingArtifactEntityTypeConfiguration :
	   IEntityTypeConfiguration<Artifact>
	{
		public void Configure(EntityTypeBuilder<Artifact> config)
		{
			config.ToTable("meeting_artifacts", "membership");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.MeetingId)
			.HasColumnName("meeting_id")
			.IsRequired();

			config.Property(o => o.Title)
			.HasColumnName("title")
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();			

			config.Property(o => o.Description)
			.HasColumnName("description")
			.HasMaxLength(600)
			.IsRequired()
			.IsUnicode();
		}
	}

	

}