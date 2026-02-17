using ECCSA.BackOffice.Server.Model.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Payment
{
    public class CurrencyEntityTypeConfiguration :
		IEntityTypeConfiguration<Currency>
	{
		public void Configure(EntityTypeBuilder<Currency> config)
		{
			config.ToTable("currencies", "payment");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasMaxLength(3)
			.IsRequired();


			config.Property(o => o.Name)
			.HasColumnName("name")
			.HasMaxLength(20)
			.IsRequired();

		}
	}
}