using ECCSA.BackOffice.Server.Model.Advocacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Advocacy
{
	public class DialogEntityTypeConfiguration:
		IEntityTypeConfiguration<Dialog>
	{
	  public void Configure(EntityTypeBuilder<Dialog> config)
	  {
			config.ToTable("dialogs", "advocacy") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.Title)
				.HasColumnName("title")
				.HasColumnType("varchar")
				.HasMaxLength(100);

			config.Property(o => o.Description)
				.HasColumnName("description")
				.HasMaxLength(800)
				.IsUnicode();


			config.Property(o => o.ResponsiblePerson)
				.HasColumnName("responsible_person")
				.HasMaxLength(100)
				.IsUnicode();


			config.Property(o => o.TargetInstitution)
				.HasColumnName("target_institution")
				.HasMaxLength(100)
				.IsUnicode();

			config.Property(o => o.Outcome)
				.HasColumnName("outcome")
				.HasMaxLength(100)
				.IsUnicode();

			config.Property(o => o.ImplementationPeriod)
				.HasColumnName("implementation_period")
				.HasMaxLength(100)
				.IsUnicode();

			config.Property(o => o.ConsultationDates)
				.HasColumnName("consultation_dates")
				.HasMaxLength(100)
				.IsUnicode();

			config.Property(o => o.Remark)
				.HasColumnName("remark")
				.HasMaxLength(100)
				.IsUnicode();

			config.Property(o => o.Status)
				.HasColumnName("status")
				.IsRequired();

			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o=> o.CreateStamp)
				.HasColumnName("create_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();

			config.Property(o => o.UpdatedBy)
				.HasColumnName("updated_by")
				.IsRequired();

			config.Property(o => o.UpdateStamp)
				.HasColumnName("update_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();
			config.HasMany<PPDAttachment>(o => o.Documents)
				.WithOne(s => s.Dialog)
				.HasForeignKey(s => s.DialogId);

			config.HasMany<PPDMeeting>(o => o.Meetings)
				.WithOne(s => s.Dialog)
				.HasForeignKey(s => s.DialogId);

			config.HasMany<Recommendation>(o => o.Recommendations)
				.WithOne(s => s.Dialog)
				.HasForeignKey(s => s.DialogId);

		}
	}

	public class DialogRecommendationEntityTypeConfiguration :
		IEntityTypeConfiguration<Recommendation>
	{
		public void Configure(EntityTypeBuilder<Recommendation> config)
		{
			config.ToTable("reform_recommendations", "advocacy");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.DialogId)
				.HasColumnName("ppd_id")
				.IsRequired();
			config.Property(o => o.No)
				.HasColumnName("no")
				.IsRequired();

			config.Property(o => o.Title)
				.HasColumnName("title")
				.HasColumnType("varchar")
				.IsUnicode()
				.HasMaxLength(500);
			config.Property(o => o.Status)
				.HasColumnName("status")
				.IsRequired();

			config.Property(o => o.ImplementationText)
			.HasColumnName("implementation_text")
			.HasMaxLength(500)
			.IsUnicode();


			config.Property(o => o.Remark)
			.HasColumnName("remark")
			.HasMaxLength(500)
			.IsUnicode();


			config.Property(o => o.ResponsibleBody)
			.HasColumnName("responsible_body")
			.HasMaxLength(250)
			.IsUnicode();
			config.Property(o => o.FollowupPerson)
			.HasColumnName("followup_person")
			.HasMaxLength(250)
			.IsUnicode();

			

			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o => o.CreateStamp)
				.HasColumnName("create_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();

			config.Property(o => o.UpdatedBy)
				.HasColumnName("updated_by")
				.IsRequired();

			config.Property(o => o.UpdateStamp)
				.HasColumnName("update_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();
			config.HasMany<MeetingPoint>(o => o.Points)
				.WithOne(s => s.Recommendation)
				.HasForeignKey(s => s.RecommendationId);
		}
	}

	public class DialogMeetingEntityTypeConfiguration :
		IEntityTypeConfiguration<PPDMeeting>
	{
		public void Configure(EntityTypeBuilder<PPDMeeting> config)
		{
			config.ToTable("dialog_meetings", "advocacy");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();

			config.Property(o => o.DialogId)
				.HasColumnName("ppd_id")
				.IsRequired();

			config.Property(o => o.Date)
			.HasColumnName("ppd_date")
			.HasColumnType("date");

			config.Property(o => o.Agendas)
			.HasColumnName("agendas")
			.HasMaxLength(1000)
			.IsUnicode();


			config.Property(o => o.Participants)
			.HasColumnName("participants")
			.HasMaxLength(1000)
			.IsUnicode();


			config.Property(o => o.ChairPersons)
			.HasColumnName("chair_persons")
			.HasMaxLength(255)
			.IsUnicode();

			config.Property(o => o.Consultant)
			.HasColumnName("consultant")
			.HasMaxLength(255)
			.IsUnicode();

			config.Property(o => o.TWGMembers)
			.HasColumnName("twg_members")
			.HasMaxLength(500)
			.IsUnicode();


			config.Property(o => o.Remark)
			.HasColumnName("remark")
			.HasMaxLength(500)
			.IsUnicode();

			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o => o.CreateStamp)
				.HasColumnName("create_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();

			config.Property(o => o.UpdatedBy)
				.HasColumnName("updated_by")
				.IsRequired();

			config.Property(o => o.UpdateStamp)
				.HasColumnName("update_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();

			config.HasMany<PPDAttachment>(o => o.Documents)
				.WithOne(s => s.Meeting)
				.HasForeignKey(s => s.MeetingId);

			config.HasMany<MeetingPoint>(o => o.Points)
				.WithOne(s => s.Meeting)
				.HasForeignKey(s => s.MeetingId);

		}
	}

	public class DialogAttachmentEntityTypeConfiguration :
		IEntityTypeConfiguration<PPDAttachment>
	{
		public void Configure(EntityTypeBuilder<PPDAttachment> config)
		{
			config.ToTable("dialog_attachments", "advocacy");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.DialogId)
				.HasColumnName("ppd_id")
				.IsRequired();

			config.Property(o => o.MeetingId)
				.HasColumnName("ppd_meeting_id")
				.IsRequired();

			config.Property(o => o.Title)
			.HasColumnName("title")
			.HasColumnType("varchar")
			.HasMaxLength(255);

			config.Property(o => o.Extension)
			.HasColumnName("extension")
			.HasMaxLength(10)
			.IsUnicode();


			config.Property(o => o.IsPublic)
			.HasColumnName("is_public");


			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o => o.CreateStamp)
				.HasColumnName("create_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();

			config.Property(o => o.UpdatedBy)
				.HasColumnName("updated_by")
				.IsRequired();

			config.Property(o => o.UpdateStamp)
				.HasColumnName("update_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();

		}
	}
	public class DialogPointEntityTypeConfiguration :
		IEntityTypeConfiguration<MeetingPoint>
	{
		public void Configure(EntityTypeBuilder<MeetingPoint> config)
		{
			config.ToTable("dialog_meeting_points", "advocacy");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();

			config.Property(o => o.MeetingId)
			.HasColumnName("dialog_meeting_id")
			.HasColumnType("uuid")
			.IsRequired();

			config.Property(o => o.RecommendationId)
			.HasColumnName("recommendation_id")
			.HasColumnType("uuid")
			.IsRequired();

			config.Property(o => o.Declaration)
				.HasColumnName("common_declaration")
				.IsRequired();

			config.Property(o => o.ResponsibleBody)
			.HasColumnName("responsible_body")
			.HasColumnType("varchar")
			.HasMaxLength(100);

			config.Property(o => o.FollowupPerson)
			.HasColumnName("followup_person")
			.HasMaxLength(255)
			.IsUnicode();


			
		}
	}
	
}