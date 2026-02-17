using ECCSA.BackOffice.Server.Model.Common.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Common.Notification
{
    public class EmailMessageEntityTypeConfiguration :
        IEntityTypeConfiguration<EmailMessage>
    {
        public void Configure(EntityTypeBuilder<EmailMessage> config)
        {
            config.ToTable("email_messages", "notification");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Subject)
                    .HasColumnName("subject")
                    .HasMaxLength(100)
                    .IsRequired()
                    .IsUnicode();
            config.Property(o => o.Owner)
                   .HasColumnName("owner")
                   .IsRequired();

            config.Property(o => o.TemplateId)
                    .HasColumnName("template_id")
                    .IsRequired();

            config.Property(o => o.ReferenceId)
                    .HasColumnName("reference_id");

            config.Property(o => o.Body)
                    .HasColumnName("body")
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.Attachment)
                .HasColumnName("attachment")
                .IsRequired()
                .IsUnicode();

            config.Property(o => o.Recipients)
                    .HasColumnName("recipients")
                    .HasColumnType("jsonb")
                    .IsRequired();

            config.Property(o => o.Status)
                .HasColumnName("status")
                .IsRequired();

            config.Property(o => o.CreatedBy)
                        .HasColumnName("created_by")
                        .IsRequired();

            config.Property(o => o.CreateStamp)
                .HasColumnName("create_stamp")
                .IsRequired();

            config.Property(o => o.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired();

            config.Property(o => o.UpdateStamp)
                .HasColumnName("update_stamp")
                .IsRequired();

        }
    }

}