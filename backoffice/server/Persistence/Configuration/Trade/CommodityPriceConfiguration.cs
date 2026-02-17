using ECCSA.BackOffice.Server.Model.Trade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class CommodityPriceEntityTypeConfiguration:
		IEntityTypeConfiguration<CommodityPrice>
	{
	  public void Configure(EntityTypeBuilder<CommodityPrice> config)
	  {
			config.ToTable("commodity_prices", "trade") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
				.HasColumnName("id")
				.HasColumnType("uuid")
				.IsRequired();


			config.Property(o => o.MarketDate)
				.HasColumnName("market_date")
				.HasColumnType("date")
				.IsRequired();


			config.Property(o => o.CommodityTypeId)
				.HasColumnName("commodity_type_id")
				.IsRequired();


			config.Property(o => o.PriceUnitRateId)
				.HasColumnName("price_unit_rate_id")
				.IsRequired();

			config.Property(o => o.MarketPlaceId)
				.HasColumnName("market_place_id")
				.IsRequired();


			config.Property(o => o.PriceValue)
				.HasColumnName("price_value")
				.HasColumnType("decimal")
				.IsRequired();

	  }
	}
}