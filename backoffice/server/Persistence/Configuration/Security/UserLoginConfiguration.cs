using ECCSA.BackOffice.Server.Model.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class UserLoginEntityTypeConfiguration:
		IEntityTypeConfiguration<UserLogin>
	{
	  public void Configure(EntityTypeBuilder<UserLogin> config)
	  {
			config.ToTable("user_logins", "security");		

			config.HasKey(o => new { o.LoginProvider , o.ProviderKey , o.UserId });

			config.Property(o => o.LoginProvider)
			.HasColumnName("login_provider")			
			.IsRequired()
			.HasMaxLength(128)
			.IsUnicode();

			config.Property(o => o.ProviderKey)
			.HasColumnName("provider_key")			
			.IsRequired()
			.HasMaxLength(128)
			.IsUnicode();

			config.Property(o => o.UserId)
			.HasColumnName("user_id")
			.HasColumnType("uuid")
			.IsRequired();

	  }
	}
}