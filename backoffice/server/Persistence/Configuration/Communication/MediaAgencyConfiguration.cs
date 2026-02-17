using ECCSA.BackOffice.Server.Model.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Communication
{
    public class MediaAgencyEntityTypeConfiguration :
        IEntityTypeConfiguration<MediaAgency>
    {
        public void Configure(EntityTypeBuilder<MediaAgency> config)
        {
            config.ToTable("media_agencies", "communication");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();


            config.Property(o => o.Name)
            .HasColumnName("name")
            .HasColumnType("varchar")
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(200);





            config.Property(o => o.MediaType)
            .HasColumnName("media_type")
            .IsRequired()
            .HasDefaultValue(MediaTypeFlag.NotSet);


            config.Property(o => o.IsLocalAgency)
            .HasColumnName("is_local_agency")
            .HasColumnType("int")
            .IsRequired()
            .HasDefaultValue(0);


            config.Property(o => o.OwnershipType)
            .HasColumnName("ownership_type")
            .IsRequired();


            config.Property(o => o.RepresentativeName)
            .HasColumnName("representative_name")
            .HasColumnType("varchar")
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(200);





            config.Property(o => o.ResponsiblePersonnel)
            .HasColumnName("responsible_personnel")
            .HasColumnType("varchar")
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(200);





            config.Property(o => o.Email)
            .HasColumnName("email")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(200);


            config.Property(o => o.Mobile)
            .HasColumnName("mobile")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(50);


            config.Property(o => o.Telephone)
            .HasColumnName("telephone")
            .HasColumnType("varchar")
            .IsRequired()
                        .HasMaxLength(50);


            config.Property(o => o.Url)
            .HasColumnName("url")
            .HasColumnType("varchar")
            .IsRequired()
                        .HasMaxLength(100);


            config.Property(o => o.Description)
            .HasColumnName("description")

                        .HasMaxLength(800)
            .IsUnicode();


            config.Property(o => o.Remark)
            .HasColumnName("remark")
            .HasMaxLength(800)
            .IsUnicode();


            config.Property(o => o.Status)
            .HasColumnName("status");


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


            config.HasMany<CampaignPartnerAgency>(o => o.CampaignPartnerAgencies)
                .WithOne(s => s.MediaAgency)
                .HasForeignKey(s => s.MediaAgencyId);


            config.HasMany<MediaCoverage>(o => o.MediaCoverages)
                .WithOne(s => s.MediaAgency)
                .HasForeignKey(s => s.MediaAgencyId);






        }
    }
}