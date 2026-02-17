using ECCSA.BackOffice.Server.Model.Common.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Common.Notification
{

    public class RecipientSourceEntityTypeConfiguration :
        IEntityTypeConfiguration<RecipientSource>
    {
        public void Configure(EntityTypeBuilder<RecipientSource> config)
        {
            config.ToTable("recipients", "notification");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.CompanyName)
                    .HasColumnName("company")
                    .HasMaxLength(100)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.MobileNo)
                    .HasColumnName("mobile")
                    .HasMaxLength(20)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.Owner)
                    .HasColumnName("owner")
                    .IsRequired();

            config.Property(o => o.SectorId)
                   .HasColumnName("sector_id")
                   .IsRequired(false);

            config.Property(o => o.ImportSectorName)
                   .HasColumnName("import_sector_name")
                    .HasMaxLength(200)
                   .IsRequired(false);

            config.Property(o => o.ImportSubsectorName)
                   .HasColumnName("import_sub_sector_name")
                    .HasMaxLength(200)
                   .IsRequired(false);

            config.Property(o => o.BusinessInterest)
                   .HasColumnName("business_interest")
                    .HasMaxLength(255)
                   .IsRequired(false);

            config.Property(o => o.Website)
                   .HasColumnName("website")
                    .HasMaxLength(200)
                   .IsRequired(false);

            config.Property(o => o.OfficeTel)
                   .HasColumnName("office_tel")
                    .HasMaxLength(20)
                   .IsRequired(false);

            config.Property(o => o.Position)
                   .HasColumnName("position")
                    .HasMaxLength(100)
                   .IsRequired(false);

            config.Property(o => o.StakeholderType)
                    .HasColumnName("stakeholder_type")
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
    public class TempRecipientSourceEntityTypeConfiguration :
        IEntityTypeConfiguration<TempRecipientSource>
    {
        public void Configure(EntityTypeBuilder<TempRecipientSource> config)
        {
            config.ToTable("temp_recipients", "notification");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            config.Property(o => o.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.CompanyName)
                    .HasColumnName("company")
                    .HasMaxLength(100)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.MobileNo)
                    .HasColumnName("mobile")
                    .HasMaxLength(20)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.Owner)
                    .HasColumnName("owner")
                    .IsRequired();

            config.Property(o => o.SectorId)
                   .HasColumnName("sector_id")
                   .IsRequired(false);

            config.Property(o => o.ImportSectorName)
                   .HasColumnName("import_sector_name")
                    .HasMaxLength(200)
                   .IsRequired(false);

            config.Property(o => o.ImportSubsectorName)
                   .HasColumnName("import_sub_sector_name")
                    .HasMaxLength(200)
                   .IsRequired(false);

            config.Property(o => o.BusinessInterest)
                   .HasColumnName("business_interest")
                    .HasMaxLength(255)
                   .IsRequired(false);

            config.Property(o => o.Website)
                   .HasColumnName("website")
                    .HasMaxLength(200)
                   .IsRequired(false);

            config.Property(o => o.OfficeTel)
                   .HasColumnName("office_tel")
                    .HasMaxLength(20)
                   .IsRequired(false);

            config.Property(o => o.Position)
                   .HasColumnName("position")
                    .HasMaxLength(100)
                   .IsRequired(false);

            config.Property(o => o.StakeholderType)
                    .HasColumnName("stakeholder_type")
                    .IsRequired();

        }
    }
}