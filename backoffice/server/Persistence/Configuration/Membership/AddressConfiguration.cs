using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class AddressEntityTypeConfiguration:
		IEntityTypeConfiguration<Address>
	{
	  public void Configure(EntityTypeBuilder<Address> config)
	  {
			config.ToTable("addresses", "membership") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.ValueGeneratedOnAdd()
			.IsRequired();


			config.Property(o => o.MemberId)
			.HasColumnName("member_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.AddressType)
			.HasColumnName("address_type")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.LocationId)
			.HasColumnName("location_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.AddressLine)
			.HasColumnName("address_line")
			
						.HasMaxLength(500)
			.IsUnicode();




		   

	  }
	}
}