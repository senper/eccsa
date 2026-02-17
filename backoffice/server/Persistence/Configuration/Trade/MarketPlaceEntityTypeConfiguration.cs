using ECCSA.BackOffice.Server.Model.Trade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class MarketPlaceEntityTypeConfiguration :
		IEntityTypeConfiguration<MarketPlace>
	{
		public void Configure(EntityTypeBuilder<MarketPlace> config)
		{
			config.ToTable("market_places", "trade");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.Code)
				.HasColumnName("code")
				.HasColumnType("varchar")
				.HasMaxLength(50)
				.IsRequired();

			config.Property(o => o.NameEng)
				.HasColumnName("name_eng")
				.HasColumnType("varchar")
				.IsRequired()
				.HasMaxLength(100);


			config.Property(o => o.NameLocal)
				.HasColumnName("name_local")
				.IsRequired()
				.HasMaxLength(100)
				.IsUnicode();

			config.Property(o => o.LocationId)
				.HasColumnName("location_id")
				.IsRequired()
				.HasDefaultValue(Guid.Parse("00000000-0000-0000-0000-0000000f4240"));
			
			config.Property(o => o.Latitude)
				.HasColumnName("latitude")
				.IsRequired()
				.HasDefaultValue(0);
			
			config.Property(o => o.Longitude)
				.HasColumnName("longitude")
				.IsRequired()
				.HasDefaultValue(0);


			config.HasMany<CommodityPrice>(o => o.CommodityPrices)
				.WithOne(s => s.MarketPlace)
				.HasForeignKey(s => s.MarketPlaceId);






		}
	}
}