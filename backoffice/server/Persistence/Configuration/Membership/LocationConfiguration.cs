using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class LocationEntityTypeConfiguration :
        IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> config)
        {
            config.ToTable("locations", "membership");
            config.HasKey(o => o.Id);
            config.Property(o => o.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd()
            .IsRequired();

            config.Property(o => o.Code)
            .HasColumnName("code")
            .HasMaxLength(20)
            .IsRequired();

            config.Property(o => o.ParentId)
                .HasColumnName("parent_id")
                .HasColumnType("uuid");

            config.Property(o => o.Level)
                .HasColumnName("level")
                .IsRequired();

            config.Property(o => o.NameEng)
            .HasColumnName("name_eng")
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode();


            config.Property(o => o.NameLocal)
            .HasColumnName("name_local")
            .HasMaxLength(200)
            .IsRequired()
            .IsUnicode();

            config.Property(o => o.Level1Id)
                .HasColumnName("level1_id")
                .HasColumnType("uuid");

            config.Property(o => o.Level1NameEng)
            .HasColumnName("level1_name_eng")
            .HasMaxLength(200)
            .IsUnicode();


            config.Property(o => o.Level1NameLocal)
            .HasColumnName("level1_name_local")
            .HasMaxLength(200)
            .IsUnicode();

            config.Property(o => o.Level2Id)
                .HasColumnName("level2_id")
                .HasColumnType("uuid");

            config.Property(o => o.Level2NameEng)
            .HasColumnName("level2_name_eng")
            .HasMaxLength(200)
            .IsUnicode();


            config.Property(o => o.Level2NameLocal)
            .HasColumnName("level2_name_local")
            .HasMaxLength(200)
            .IsUnicode();


            

            config.HasMany<Association>(o => o.Associations)
                .WithOne(s => s.Location)
                .HasForeignKey(s => s.LocationId);

            config.HasMany<Individual>(o => o.Individuals)
                .WithOne(s => s.Location)
                .HasForeignKey(s => s.LocationId);

        }
    }
}