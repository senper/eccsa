using ECCSA.BackOffice.Server.Model.Common.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Common.Notification
{
    public class EmailTemplateEntityTypeConfiguration :
        IEntityTypeConfiguration<EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<EmailTemplate> config)
        {
            config.ToTable("email_templates", "notification");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.Owner)
                    .HasColumnName("owner")
                    .IsRequired();

            config.Property(o => o.PlaceHolders)
                    .HasColumnName("place_holders")
                    .HasColumnType("jsonb")
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