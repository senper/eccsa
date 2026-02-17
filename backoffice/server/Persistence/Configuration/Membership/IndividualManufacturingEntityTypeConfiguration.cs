using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class IndividualManufacturingEntityTypeConfiguration :
	   IEntityTypeConfiguration<IndividualManufacturing>
	{
		public void Configure(EntityTypeBuilder<IndividualManufacturing> config)
		{
			config.ToTable("individual_manufacturing", "membership");


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


			config.Property(o => o.Product)
			.HasColumnName("product")
			.HasMaxLength(250)
			.IsRequired()
			.IsUnicode();

			config.Property(o => o.Unit)
			.HasColumnName("unit")
			.HasMaxLength(250)
			.IsRequired()
			.IsUnicode();

			config.Property(o => o.ProductionCapacity)
			.HasColumnName("production_capacity")
			.HasMaxLength(250)
			.IsRequired()
			.IsUnicode();

			config.Property(o => o.UtilizedCapacity)
			.HasColumnName("utilized_capacity")
			.HasMaxLength(250)
			.IsRequired()
			.IsUnicode();





		}
	}
}