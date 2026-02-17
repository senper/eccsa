using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class RepresentativeContactEntityTypeConfiguration:
        IEntityTypeConfiguration<RepresentativeContact>
    {
      public void Configure(EntityTypeBuilder<RepresentativeContact> config)
      {
            config.ToTable("representative_contacts", "membership") ;
            

            config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.RepresentativeId)
			.HasColumnName("representative_id")
			.HasColumnType("uuid");
			

			config.Property(o => o.ContactType)
			.HasColumnName("contact_type")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.Address)
			.HasColumnName("address")
			
			.IsRequired()
			
			.IsUnicode();


			config.Property(o => o.Priority)
			.HasColumnName("priority")
			.HasColumnType("int")
			.IsRequired();




           

      }
    }
}