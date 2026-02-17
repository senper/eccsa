using ECCSA.BackOffice.Server.Model.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Payment
{
    public class DiscountCategoryEntityTypeConfiguration :
		IEntityTypeConfiguration<DiscountCategory>
	{
		public void Configure(EntityTypeBuilder<DiscountCategory> config)
		{
			config.ToTable("discount_categories", "payment");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();


			config.Property(o => o.Rate)
			.HasColumnName("rate")
			.IsRequired();

		}
	}
}