using ECCSA.BackOffice.Server.Model.Trade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class ExchangeRateEntityTypeConfiguration:
		IEntityTypeConfiguration<ExchangeRate>
	{
	  public void Configure(EntityTypeBuilder<ExchangeRate> config)
	  {
			config.ToTable("exchange_rates", "trade") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();


			config.Property(o => o.TransactionDate)
				.HasColumnName("transaction_date")
				.HasColumnType("date")
				.IsRequired();


			config.Property(o => o.Currency)
				.HasColumnName("currency")
				.HasColumnType("varchar")
				.IsRequired()
				.HasMaxLength(50);




			config.Property(o => o.Buying)
				.HasColumnName("buying")
				.HasColumnType("decimal")
				.IsRequired();


			config.Property(o => o.Selling)
				.HasColumnName("selling")
				.HasColumnType("decimal")
				.IsRequired();




		   

	  }
	}
}