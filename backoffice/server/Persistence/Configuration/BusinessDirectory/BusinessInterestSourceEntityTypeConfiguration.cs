using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.BusinessDirectory
{
    public class BusinessInterestSourceEntityTypeConfiguration :
        IEntityTypeConfiguration<BusinessInterestSource>
    {
        public void Configure(EntityTypeBuilder<BusinessInterestSource> config)
        {
            config.ToTable("business_interest_sources", "directory");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();

            config.Property(o => o.Name)
            .HasColumnName("name")
            .HasColumnType("varchar")
            .IsUnicode()
            .IsRequired()
            .HasMaxLength(255);

            config.Property(o => o.Telephone)
                .HasColumnName("telephone")
            .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);

            config.Property(o => o.Email)
                .HasColumnName("email")
            .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);

            config.Property(o => o.Description)
                .HasColumnName("description")
                .HasColumnType("varchar")
            .IsRequired()
                .HasMaxLength(800);

            config.Property(o => o.UserId)
                .HasColumnName("user_id");

            config.Property(o => o.Address)
                .HasColumnName("address");

            config.HasMany<BusinessInterestSourceSector>(o => o.Sectors)
                .WithOne(s => s.BusinessInterestSource)
                .HasForeignKey(s => s.BusinessInterestSourceId);

            config.HasMany<BusinessInterestSourceDetail>(o => o.Details)
                .WithOne(s => s.BusinessInterestSource)
                .HasForeignKey(s => s.BusinessInterestSourceId);

            config.HasMany<MatchMaking>(o => o.Matches)
                .WithOne(s => s.Source)
                .HasForeignKey(s => s.SourceId);
        }
    }
}