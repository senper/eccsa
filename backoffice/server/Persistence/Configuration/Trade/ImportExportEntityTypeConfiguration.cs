using ECCSA.BackOffice.Server.Model.Trade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class ImportExportEntityTypeConfiguration :
		IEntityTypeConfiguration<ImportExport>
	{
		public void Configure(EntityTypeBuilder<ImportExport> config)
		{
			config.ToTable("import_exports", "trade");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.HSCode)
				.HasColumnName("hscode")
				.HasColumnType("varchar")
				.HasMaxLength(50)
				.IsRequired();

			config.Property(o => o.CPCode)
				.HasColumnName("cpcode")
				.HasColumnType("varchar")
				.IsRequired()
				.HasMaxLength(10);


			config.Property(o => o.CurrentEPC)
				.HasColumnName("current_epc")
				.IsRequired()
				.HasMaxLength(10)
				.IsUnicode();

			config.Property(o => o.PreviousEPC)
				.HasColumnName("previous_epc")
				.IsRequired()
				.HasMaxLength(10)
				.IsUnicode();

			config.Property(o => o.APC)
				.HasColumnName("apc")
				.IsRequired()
				.HasMaxLength(10)
				.IsUnicode();

			config.Property(o => o.Direction)
				.HasColumnName("direction")
				.IsRequired()
				.HasMaxLength(2)
				.IsUnicode();

			config.Property(o => o.StartDate)
				.HasColumnName("start_date")
				.IsRequired()
				.HasColumnType("date");


			config.Property(o => o.EndDate)
				.HasColumnName("end_date")
				.IsRequired()
				.HasColumnType("date");


			config.Property(o => o.OriginCountry)
				.HasColumnName("origin_country")
				.IsRequired()
				.HasMaxLength(10)
				.IsUnicode();

			config.Property(o => o.DestinationCountry)
				.HasColumnName("destination_country")
				.IsRequired()
				.HasMaxLength(10)
				.IsUnicode();

			config.Property(o => o.ConsignmentCountry)
				.HasColumnName("consignment_country")
				.IsRequired()
				.HasMaxLength(10)
				.IsUnicode();

			config.Property(o => o.NetWeightKg)
				.HasColumnName("net_weight_kg")
				.IsRequired();


			config.Property(o => o.TradeValue)
				.HasColumnName("trade_value")
				.IsRequired();


			config.Property(o => o.TaxPaid)
				.HasColumnName("tax_paid")
				.IsRequired();


			config.Property(o => o.NumberOfTrades)
				.HasColumnName("trades")
				.IsRequired();








		}
	}
}