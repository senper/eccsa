using ECCSA.BackOffice.Server.Model.Trade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class CommodityTypeEntityTypeConfiguration:
		IEntityTypeConfiguration<CommodityType>
	{
	  public void Configure(EntityTypeBuilder<CommodityType> config)
	  {
			config.ToTable("commodity_types", "trade") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();


			config.Property(o => o.Code)
				.HasColumnName("code")
				.HasColumnType("varchar")
				.IsRequired()
				.HasMaxLength(50);

			config.Property(o => o.Name)
				.HasColumnName("name")
				.HasColumnType("varchar")
				.IsRequired()
				.HasMaxLength(100);


			config.Property(o => o.NameLocal)
				.HasColumnName("name_local")
				.IsRequired()
				.HasDefaultValue(" ")
				.HasMaxLength(100)
				.IsUnicode();


			config.HasMany<CommodityPrice>(o => o.CommodityPrices)
				.WithOne(s => s.CommodityType)
				.HasForeignKey(s => s.CommodityTypeId);

	  }
	}
}