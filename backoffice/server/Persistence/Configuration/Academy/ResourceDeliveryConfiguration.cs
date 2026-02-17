using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class ResourceDeliveryEntityTypeConfiguration:
		IEntityTypeConfiguration<ResourceDelivery>
	{
	  public void Configure(EntityTypeBuilder<ResourceDelivery> config)
	  {
			config.ToTable("resource_delivery", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.DeliveryMethod)
			.HasColumnName("delivery_method")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.ResourceId)
			.HasColumnName("resource_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.RegistrationId)
			.HasColumnName("registration_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.DeliveryDate)
			.HasColumnName("delivery_date")
			
			.IsRequired();




		   

	  }
	}
}