using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class TrainingEntityTypeConfiguration :
        IEntityTypeConfiguration<Training>
    {
        public void Configure(EntityTypeBuilder<Training> config)
        {
            config.ToTable("trainings", "training");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();


            config.Property(o => o.TopicId)
            .HasColumnName("topic_id")
            .HasColumnType("uuid")
            .IsRequired();


            config.Property(o => o.StartDate)
            .HasColumnName("start_date")
            .HasColumnType("date")
            .IsRequired();


            config.Property(o => o.EndDate)
            .HasColumnName("end_date")
            .HasColumnType("date")
            .IsRequired();


            config.Property(o => o.Instructor)
            .HasColumnName("instructor").IsRequired()
            .HasMaxLength(200)
            .IsUnicode();

            config.Property(o => o.InstructionMedia)
            .HasColumnName("instruction_media")
            .IsRequired();

            config.Property(o => o.Status)
            .HasColumnName("status")
            .HasColumnType("int")
            .IsRequired();

            config.Property(o => o.RateBasis)
            .HasColumnName("rate_basis")
            .HasDefaultValue(RateBasisEnum.NotSet)
            .IsRequired();

            config.Property(o => o.MemberFee)
            .HasColumnName("member_fee")
            .HasColumnType("decimal")
            .IsRequired();

            config.Property(o => o.NonMemberFee)
            .HasColumnName("non_member_fee")
            .HasColumnType("decimal")
            .IsRequired();

            config.Property(o => o.Schedule)
            .HasColumnName("schedule").IsRequired()
            .HasMaxLength(200)
            .IsUnicode();

            config.Property(o => o.ServiceTypeId)
            .HasColumnName("service_type_id");

            config.Property(o => o.SurveyId)
                    .HasColumnName("survey_id");

            config.Property(o => o.HasMorning)
                    .HasColumnName("morning_training")
                    .HasDefaultValue(false);

            config.Property(o => o.HasAfternoon)
                    .HasColumnName("afternoon_training")
                    .HasDefaultValue(false);

            config.Property(o => o.HasEvening)
                    .HasColumnName("evening_training")
                    .HasDefaultValue(false);

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


            config.HasMany<Assessment>(o => o.Assessments)
                .WithOne(s => s.Training)
                .HasForeignKey(s => s.TrainingId);


            config.HasMany<Attendance>(o => o.Attendances)
                .WithOne(s => s.Training)
                .HasForeignKey(s => s.TrainingId);


            config.HasMany<Registration>(o => o.Registrations)
                .WithOne(s => s.Training)
                .HasForeignKey(s => s.TrainingId);

            config.HasMany<TrainingSponsor>(o => o.TrainingSponsors)
                .WithOne(s => s.Training)
                .HasForeignKey(s => s.TrainingId);






        }
    }
}