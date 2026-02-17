using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.BusinessDirectory
{
    public class TempCompanyEntityTypeConfiguration :
		IEntityTypeConfiguration<TempCompany>
	{
		public void Configure(EntityTypeBuilder<TempCompany> config)
		{
			config.ToTable("tmp_business_sector_companies", "directory");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();
			config.Property(o => o.SectorId)
			.HasColumnName("business_sector_id")
			.HasColumnType("uuid")
			.IsRequired();

			config.Property(o => o.Name)
			.HasColumnName("name")
			.HasColumnType("varchar")
			.IsUnicode()
			.IsRequired()
			.HasMaxLength(200);

			config.Property(o => o.Telephone)
				.HasColumnName("telephone")
				.HasColumnType("varchar")
				.HasMaxLength(100);
			config.Property(o => o.Mobile)
				.HasColumnName("mobile")
				.HasColumnType("varchar")
				.HasMaxLength(100);
			config.Property(o => o.Fax)
				.HasColumnName("fax")
				.HasColumnType("varchar")
				.HasMaxLength(100);
			config.Property(o => o.Email)
				.HasColumnName("email")
				.HasColumnType("varchar")
				.HasMaxLength(100);
			config.Property(o => o.PoBox)
				.HasColumnName("pobox")
				.HasColumnType("varchar")
				.HasMaxLength(100);
			config.Property(o => o.Web)
				.HasColumnName("web")
				.HasColumnType("varchar")
				.HasMaxLength(100);
			config.Property(o => o.Extension)
				.HasColumnName("extension")
				.HasColumnType("varchar")
				.HasMaxLength(100);

			






		}
	}
}