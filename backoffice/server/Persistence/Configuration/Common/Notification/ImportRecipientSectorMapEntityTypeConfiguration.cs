using ECCSA.BackOffice.Server.Model.Common.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Common.Notification
{
    public class ImportRecipientSectorMapEntityTypeConfiguration :
       IEntityTypeConfiguration<ImportRecipientSectorMap>
    {
        public void Configure(EntityTypeBuilder<ImportRecipientSectorMap> config)
        {
            config.ToTable("import_recipients_sector_map", "notification");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .UseIdentityByDefaultColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();

            config.Property(o => o.SectorId)
                   .HasColumnName("sector_id")
                   .IsRequired();

            config.Property(o => o.ImportSectorName)
                   .HasColumnName("import_sector_name")
                    .HasMaxLength(200)
                   .IsRequired();
        }
    }
}