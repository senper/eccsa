using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class SurveyEntityTypeConfiguration :
        IEntityTypeConfiguration<Survey>
    {
        public void Configure(EntityTypeBuilder<Survey> config)
        {
            config.ToTable("surveys", "surveying");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

            config.Property(o => o.SettingId)
                .HasColumnName("setting_id")
                .IsRequired();
            config.Property(o => o.Title)
                    .HasColumnName("title")
                    .HasMaxLength(100)
                    .IsUnicode()
                    .IsRequired();

            config.Property(o => o.Description)
                    .HasColumnName("description")
                    .IsUnicode()
                    .IsRequired();

            config.Property(o => o.GroupId)
                    .HasColumnName("group_id");

            config.Property(o => o.StartDate)
                .HasColumnName("start_date")
                .HasColumnType("date")
                .IsRequired();

            config.Property(o => o.EndDate)
                .HasColumnName("end_date")
                .HasColumnType("date")
                .IsRequired();



            config.Property(o => o.AnalysisDescription)
                    .HasColumnName("analysis_description")
                    .IsUnicode()
                    .IsRequired();

            config.Property(o => o.Attachment)
                .HasColumnName("attachment");

            config.Property(o => o.TelegramPublishDate)
                .HasColumnName("telegram_publish_date");

            config.Property(o => o.PollIds)
                    .HasColumnName("poll_ids")
                    .HasColumnType("jsonb")
                    .IsRequired(false);

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

            config.HasMany<Attachment>(o => o.Attachments)
                .WithOne(s => s.Survey)
                .HasForeignKey(s => s.SurveyId);

            config.HasMany<SurveyAudience>(o => o.Audiences)
                .WithOne(s => s.Survey)
                .HasForeignKey(s => s.SurveyId);

            config.HasMany<SurveyRespondent>(o => o.Respondents)
                .WithOne(s => s.Survey)
                .HasForeignKey(s => s.SurveyId);

            config.HasMany<SurveyResponse>(o => o.Responses)
                .WithOne(s => s.Survey)
                .HasForeignKey(s => s.SurveyId);

            config.HasMany<SurveyVote>(o => o.Votes)
                .WithOne(s => s.Survey)
                .HasForeignKey(s => s.SurveyId);

        }
    }
}