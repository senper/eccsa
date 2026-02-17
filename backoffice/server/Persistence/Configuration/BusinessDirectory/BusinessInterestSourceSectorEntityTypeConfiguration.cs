using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.BusinessDirectory
{
    public class BusinessInterestSourceSectorEntityTypeConfiguration :
		IEntityTypeConfiguration<BusinessInterestSourceSector>
	{
		public void Configure(EntityTypeBuilder<BusinessInterestSourceSector> config)
		{
			config.ToTable("business_interest_sectors", "directory");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
				.HasColumnName("id")
				.HasColumnType("uuid")
				.IsRequired();

			config.Property(o => o.SectorId)
				.HasColumnName("business_sector_id")
				.HasColumnType("uuid")
				.IsRequired();

			config.Property(o => o.BusinessInterestSourceId)
				.HasColumnName("business_interest_source_id")
				.HasColumnType("uuid")
				.IsRequired();
		}
	}
}