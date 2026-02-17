using ECCSA.BackOffice.Server.Model.Arbitration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Arbitration
{
    public class ArbitrationRelatedPartyEntityTypeConfiguration :
			IEntityTypeConfiguration<ArbitrationRelatedParty>
	{
		public void Configure(EntityTypeBuilder<ArbitrationRelatedParty> config)
		{
			config.ToTable("related_parties", "arbitration");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd()
			.IsRequired();

			config.Property(o => o.ServiceId)
				.HasColumnName("service_id")
				.IsRequired();

			config.Property(o => o.OrderNo)
				.HasColumnName("order_no")
				.HasColumnType("varchar")
				.HasMaxLength(10);


			config.Property(o => o.Name)
					.HasColumnName("name")
					.HasMaxLength(100)
					.IsRequired()
					.IsUnicode();

			config.Property(o => o.Address)
					.HasColumnName("address")
					.HasMaxLength(500)
					.IsUnicode();
			config.Property(o => o.TINumber)
					.HasColumnName("tin")
					.HasMaxLength(20);

		}
	}
}