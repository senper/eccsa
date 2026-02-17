using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class ContactEntityTypeConfiguration:
        IEntityTypeConfiguration<Contact>
    {
      public void Configure(EntityTypeBuilder<Contact> config)
      {
            config.ToTable("contacts", "membership") ;
            

            config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.MemberId)
			.HasColumnName("member_id")
			.HasColumnType("uuid")
			.IsRequired();


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