using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.BusinessDirectory
{
    public class BusinessInterestSourceDetailEntityTypeConfiguration :
		IEntityTypeConfiguration<BusinessInterestSourceDetail>
	{
		public void Configure(EntityTypeBuilder<BusinessInterestSourceDetail> config)
		{
			config.ToTable("source_details", "directory");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
				.HasColumnName("id")
				.HasColumnType("uuid")
				.IsRequired();

			config.Property(o => o.ProductTypeId)
				.HasColumnName("product_type")
				.HasColumnType("uuid");

			config.Property(o => o.BusinessType)
				.HasColumnName("business_type")
				.HasColumnType("uuid");

			config.Property(o => o.BusinessInterestSourceId)
				.HasColumnName("source_id")
				.HasColumnType("uuid")
				.IsRequired();

			config.Property(o => o.Description)
				.HasColumnName("description")
				.HasColumnType("varchar")
				.HasMaxLength(255);

			config.Property(o => o.DescriptionNormalized)
				.HasColumnName("description_normalized")
				.HasColumnType("varchar")
				.HasMaxLength(255);
		}
	}
}