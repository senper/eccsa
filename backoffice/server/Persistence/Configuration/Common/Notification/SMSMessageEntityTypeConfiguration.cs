using ECCSA.BackOffice.Server.Model.Common.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Common.Notification
{
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