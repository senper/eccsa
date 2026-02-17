using ECCSA.BackOffice.Server.Model.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class UserContactEntityTypeConfiguration:
        IEntityTypeConfiguration<UserContact>
    {
      public void Configure(EntityTypeBuilder<UserContact> config)
      {
            config.ToTable("user_contacts", "security");          

            config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();

			config.Property(o => o.UserId)
			.HasColumnName("user_id")
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