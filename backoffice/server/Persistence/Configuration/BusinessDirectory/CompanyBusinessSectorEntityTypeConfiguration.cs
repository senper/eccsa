using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.BusinessDirectory
{
    public class CompanyBusinessSectorEntityTypeConfiguration :
		IEntityTypeConfiguration<BusinessSectorCompany>
	{
		public void Configure(EntityTypeBuilder<BusinessSectorCompany> config)
		{
			config.ToTable("business_sector_companies", "directory");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
				.HasColumnName("id")
				.HasColumnType("uuid")
				.IsRequired();

			config.Property(o => o.SectorId)
				.HasColumnName("sector_id")
				.HasColumnType("uuid")
				.IsRequired();

			config.Property(o => o.CompanyId)
				.HasColumnName("company_id")
				.HasColumnType("uuid")
				.IsRequired();
		}
	}
}