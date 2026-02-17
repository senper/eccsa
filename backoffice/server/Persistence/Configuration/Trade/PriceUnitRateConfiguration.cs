using ECCSA.BackOffice.Server.Model.Trade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class PriceUnitRateEntityTypeConfiguration:
		IEntityTypeConfiguration<PriceUnitRate>
	{
	  public void Configure(EntityTypeBuilder<PriceUnitRate> config)
	  {
			config.ToTable("price_unit_rates", "trade") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.Code)
				.HasColumnName("code")
				.HasColumnType("varchar")
				.HasMaxLength(50)
				.IsRequired();

			config.Property(o => o.Name)
				.HasColumnName("name")
				.HasColumnType("varchar")
				.IsRequired()
				.HasMaxLength(100);


			config.Property(o => o.NameLocal)
				.HasColumnName("name_local")
				.IsRequired()
				.HasMaxLength(100)
				.IsUnicode();


			config.HasMany<CommodityPrice>(o => o.CommodityPrices)
				.WithOne(s => s.PriceUnitRate)
				.HasForeignKey(s => s.PriceUnitRateId);




		   

	  }
	}
}