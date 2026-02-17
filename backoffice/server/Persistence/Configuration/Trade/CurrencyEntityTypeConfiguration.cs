using ECCSA.BackOffice.Server.Model.Trade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class CurrencyEntityTypeConfiguration :
	   IEntityTypeConfiguration<Currency>
	{
		public void Configure(EntityTypeBuilder<Currency> config)
		{
			config.ToTable("currencies", "trade");


			config.HasKey(o => o.Code);


			config.Property(o => o.Code)
				.HasColumnName("code")
				.HasColumnType("varchar")
				.HasMaxLength(50)
				.IsRequired();


			config.Property(o => o.OrderNo)
				.HasColumnName("order_no")
				.HasColumnType("varchar")
				.IsRequired()
				.HasMaxLength(10);

			config.Property(o => o.Name)
				.HasColumnName("name")
				.HasColumnType("varchar")
				.HasMaxLength(100)
				.IsRequired();



		}
	}
}