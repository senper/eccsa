using iDFEmailService.Model;
using iDFEmailService.Model.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iDFEmailService.Persistence
{
    public class MailSettingsInfoEntityTypeConfiguration :
      IEntityTypeConfiguration<MailSettings>
    {
        public void Configure(EntityTypeBuilder<MailSettings> config)
        {
            config.ToTable("mail_settings", "notification");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Mail)
                .HasColumnName("mail")
                .IsRequired();

            config.Property(o => o.Password)
                .HasColumnName("password")
                .IsRequired();

            config.Property(o => o.Host)
                .HasColumnName("host")
                .IsRequired();

            config.Property(o => o.SMTPPort)
                .HasColumnName("smtp_port")
                .IsRequired();

            config.Property(o => o.IMAPPort)
                .HasColumnName("imap_port")
                .IsRequired();

            config.Property(o => o.MaxEmailPerDay)
                .HasColumnName("max_email_per_day")
                .IsRequired();

            config.Property(o => o.CreateStamp)
                .HasColumnName("create_stamp")
                .IsRequired();

            config.Property(o => o.ActivatedStamp)
                .HasColumnName("activated_stamp")
                .IsRequired(false);

            config.Property(o => o.DeactivatedStamp)
                .HasColumnName("deactivated_stamp")
                .IsRequired(false);
        }
    }
    public class EmailDelieveryInfoEntityTypeConfiguration :
      IEntityTypeConfiguration<EmailDeliveryInfo>
    {
        public void Configure(EntityTypeBuilder<EmailDeliveryInfo> config)
        {
            config.ToTable("email_deliveries", "notification");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.SenderEmail)
                .HasColumnName("sender_email")
                .IsRequired();

            config.Property(o => o.Date)
                .HasColumnName("date")
                .HasColumnType("date")
                .IsRequired();

            config.Property(o => o.Count)
                    .HasColumnName("count")
                    .IsRequired();
          

            config.Property(o => o.Details)
                    .HasColumnName("detail")
                    .HasColumnType("jsonb")
                    .IsRequired();
        }
    }
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

            config.Property(o => o.IdentityId)
                .HasColumnName("identity_id")
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

    public class SMSMessageEntityTypeConfiguration :
        IEntityTypeConfiguration<SMSMessage>
    {
        public void Configure(EntityTypeBuilder<SMSMessage> config)
        {
            config.ToTable("sms_messages", "notification");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Message)
                    .HasColumnName("message")
                    .HasMaxLength(200)
                    .IsRequired()
                    .IsUnicode();
            config.Property(o => o.Owner)
                   .HasColumnName("owner")
                   .IsRequired();

            config.Property(o => o.Recipients)
                    .HasColumnName("recipients")
                    .HasColumnType("jsonb")
                    .IsRequired();

            config.Property(o => o.Status)
                .HasColumnName("status")
                .IsRequired();

            config.Property(o => o.Priority)
                .HasColumnName("priority")
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
