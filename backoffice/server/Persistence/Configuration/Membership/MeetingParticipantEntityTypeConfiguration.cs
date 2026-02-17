using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class MeetingParticipantEntityTypeConfiguration :
	   IEntityTypeConfiguration<Participant>
	{
		public void Configure(EntityTypeBuilder<Participant> config)
		{
			config.ToTable("meeting_participants", "membership");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.MeetingId)
			.HasColumnName("meeting_id")
			.IsRequired();

			config.Property(o => o.FullName)
			.HasColumnName("full_name")
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();

			config.Property(o => o.Email)
			.HasColumnName("email")
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();
			config.Property(o => o.Mobile)
			.HasColumnName("mobile")
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();
			config.Property(o => o.AssociationId)
			.HasColumnName("association_id")
			.IsRequired();
			config.Property(o => o.OtherParticipants)
				.HasColumnName("other_participants")
				.HasColumnType("jsonb")
				.IsRequired();

			config.Property(o => o.MinuteStatus)
			.HasColumnName("minute_status")
			.IsRequired();

			config.Property(o => o.Code)
			.HasColumnName("code")
			.HasMaxLength(200)
			.IsUnicode();

			config.Property(o => o.Remark)
			.HasColumnName("remark")
			.HasMaxLength(500)
			.IsUnicode();
		}
	}
}