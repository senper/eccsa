using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class MeetingEntityTypeConfiguration :
		IEntityTypeConfiguration<Meeting>
	{
		public void Configure(EntityTypeBuilder<Meeting> config)
		{
			config.ToTable("meetings", "membership");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.Code)
			.HasColumnName("code")
			.HasMaxLength(50)
			.IsRequired();
			config.Property(o => o.Status)
			.HasColumnName("status")
			.IsRequired();

			config.Property(o => o.Title)
					.HasColumnName("title")
					.IsRequired()
					.HasMaxLength(200)
					.IsUnicode();

			config.Property(o => o.Date)
			.HasColumnName("meeting_date")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.Venue)
					.HasColumnName("venue")
					.IsRequired()
					.HasMaxLength(200)
					.IsUnicode();
			config.Property(o => o.Agenda)
					.HasColumnName("agendas")
					.IsRequired()
					.IsUnicode();

			config.Property(o => o.RegistrationDeadline)
			.HasColumnName("registration_deadline")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.MinuteDeadline)
				.HasColumnName("minute_deadline")
				.HasColumnType("date")
				.IsRequired();

			config.Property(o => o.DistinguishedGuests)
				.HasColumnName("distinguished_guests")
				.HasColumnType("jsonb")
				.IsRequired();
			
			config.Property(o => o.CreatedBy)
					.HasColumnName("created_by")
					.IsRequired();

			config.Property(o => o.CreateStamp)
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

			config.HasMany<Artifact>(o => o.Artifacts)
				.WithOne(s => s.Meeting)
				.HasForeignKey(s => s.MeetingId);

			config.HasMany<Participant>(o => o.Participants)
				.WithOne(s => s.Meeting)
				.HasForeignKey(s => s.MeetingId);

			config.HasMany<MeetingAttendance>(o => o.Attendances)
				.WithOne(s => s.Meeting)
				.HasForeignKey(s => s.MeetingId);

			config.HasMany<Nominee>(o => o.Nominees)
				.WithOne(s => s.Meeting)
				.HasForeignKey(s => s.MeetingId);
		}
	}
	public class MeetingNomineeEntityTypeConfiguration :
	  IEntityTypeConfiguration<Nominee>
	{
		public void Configure(EntityTypeBuilder<Nominee> config)
		{
			config.ToTable("meeting_nominees", "membership");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.MeetingId)
			.HasColumnName("meeting_id")
			.IsRequired();

			config.Property(o => o.Name)
			.HasColumnName("name")
			.IsRequired()
			.HasMaxLength(250)
			.IsUnicode();

			config.Property(o => o.AssociationId)
			.HasColumnName("association_id")
			.IsRequired();

			config.Property(o => o.Position)
			.HasColumnName("position")
			.IsRequired()
			.HasMaxLength(250)
			.IsUnicode();

			config.Property(o => o.YeaVotes)
			.HasColumnName("yeavote")
			.IsRequired();

			config.Property(o => o.NayVotes)
			.HasColumnName("nayvote")
			.IsRequired();

			config.Property(o => o.AbstainVotes)
			.HasColumnName("abstainvote")
			.IsRequired();

			
		}
	}

	public class MeetingAttendanceEntityTypeConfiguration :
	  IEntityTypeConfiguration<MeetingAttendance>
	{
		public void Configure(EntityTypeBuilder<MeetingAttendance> config)
		{
			config.ToTable("meeting_attendances", "membership");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.MeetingId)
			.HasColumnName("meeting_id")
			.IsRequired();

			config.Property(o => o.No)
			.HasColumnName("no")
			.IsRequired()
			.HasMaxLength(10)
			.IsUnicode();

			config.Property(o => o.ParticipantId)
			.HasColumnName("participant_id")
			.IsRequired();

			config.Property(o => o.AttendaceDate)
			.HasColumnName("attendance_date")
			.HasColumnType("date")
			.IsUnicode();



		}
	}
}