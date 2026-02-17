using ECCSA.BackOffice.Server.Model.Investment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Investment
{
    public class RegionSectorEntityTypeConfiguration :
        IEntityTypeConfiguration<RegionSector>
    {
        public void Configure(EntityTypeBuilder<RegionSector> config)
        {
            config.ToTable("region_sectors", "investment");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();


            config.Property(o => o.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode();

            config.Property(o => o.NameLocal)
                    .HasColumnName("name_local")
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode();

            config.Property(o => o.IsRegion)
            .HasColumnName("is_region")
            .IsRequired();
            config.Property(o => o.Profile)
                .HasColumnName("profile")
                .IsRequired()
                .IsUnicode()
                .HasDefaultValue("");
            config.Property(o => o.ProfileLocal)
                .HasColumnName("profile_local")
                .IsRequired()
                .IsUnicode()
                .HasDefaultValue("");






        }
    }
}