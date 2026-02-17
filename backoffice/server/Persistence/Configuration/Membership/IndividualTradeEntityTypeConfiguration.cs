using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class IndividualTradeEntityTypeConfiguration :
	   IEntityTypeConfiguration<IndividualTrade>
	{
		public void Configure(EntityTypeBuilder<IndividualTrade> config)
		{
			config.ToTable("individual_trades", "membership");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.IndividualId)
			.HasColumnName("individual_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.SerNo)
			.HasColumnName("ser_no")
			.HasMaxLength(10)
			.IsRequired();


			config.Property(o => o.TradeDirection)
			.HasColumnName("trade_direction")
			.HasMaxLength(1)
			.IsRequired()
			.IsUnicode();

			config.Property(o => o.ProductName)
			.HasColumnName("product_name")
			.HasMaxLength(250)
			.IsRequired()
			.IsUnicode();

			config.Property(o => o.Country)
			.HasColumnName("country")
			.HasMaxLength(250)
			.IsRequired()
			.IsUnicode();

			




		}
	}
}