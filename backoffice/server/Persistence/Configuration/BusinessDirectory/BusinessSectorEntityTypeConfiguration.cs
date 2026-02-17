using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.BusinessDirectory
{
    public class BusinessSectorEntityTypeConfiguration :
		IEntityTypeConfiguration<BusinessSector>
	{
		public void Configure(EntityTypeBuilder<BusinessSector> config)
		{
			config.ToTable("business_sectors", "directory");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
				.HasColumnName("id")
				.HasColumnType("uuid")
				.IsRequired();

			config.Property(o => o.ParentId)
				.HasColumnName("parent_id")
				.HasColumnType("uuid")
				.IsRequired(false);

			config.Property(o => o.Name)
				.HasColumnName("name")
				.HasMaxLength(200)
				.IsUnicode()
				.IsRequired();


			config.HasMany<BusinessSector>(o => o.SubSectors)
				.WithOne(s => s.ParentSector)
				.HasForeignKey(s => s.ParentId);

			config.HasMany<BusinessSectorCompany>(o => o.BusinessSectors)
				.WithOne(s => s.BusinessSector)
				.HasForeignKey(s => s.SectorId);

			config.HasMany<BusinessInterestSourceSector>(o => o.BusinessInterestSourceSectors)
				.WithOne(s => s.BusinessSector)
				.HasForeignKey(s => s.SectorId);

			config.HasMany<TempCompany>(o => o.TempCompanies)
				.WithOne(s => s.BusinessSector)
				.HasForeignKey(s => s.SectorId);

		}
	}
}