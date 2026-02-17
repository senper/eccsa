using ECCSA.BackOffice.Server.Model.PublicSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class PublicUserEntityTypeConfiguration :
		IEntityTypeConfiguration<PublicUser>
	{
	  public void Configure(EntityTypeBuilder<PublicUser> config)
	  {
			config.ToTable("users", "security") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();

			config.Property(o => o.MemberId)
				.HasColumnName("member_id");


			config.Property(o => o.UserName)
			.HasColumnName("user_name")
			
			.IsRequired()
			.HasMaxLength(100)
			.IsUnicode();


			config.Property(o => o.Email)
			.HasColumnName("email")
			
			.IsRequired()
			.HasMaxLength(100)
			.IsUnicode();


			config.Property(o => o.PasswordHash)
			.HasColumnName("password_hash")
			
						
			.IsUnicode();


			config.Property(o => o.SecurityStamp)
			.HasColumnName("security_stamp")
			
						
			.IsUnicode();


			config.Property(o => o.PhoneNumber)
			.HasColumnName("phone_number")
			.HasColumnType("varchar")
						.HasMaxLength(20);


			config.Property(o => o.EmailConfirmed)
			.HasColumnName("email_confirmed")
			
			.IsRequired();


			config.Property(o => o.PhoneNumberConfirmed)
			.HasColumnName("phone_number_confirmed")
			
			.IsRequired();


			config.Property(o => o.TwoFactorEnabled)
			.HasColumnName("two_factor_enabled")
			
			.IsRequired();


			config.Property(o => o.LockoutEnabled)
			.HasColumnName("lockout_enabled")
			.IsRequired();

			config.Property(o => o.AccessFailedCount)
			.HasColumnName("access_failed_count")
			.HasColumnType("int")
			.IsRequired();

			config.Property(o => o.LockoutEndDateUtc)
			.HasColumnName("lockout_end_date_utc");

			config.Property(o => o.NormalizedEmail)
			.HasColumnName("normalized_email");

			config.Property(o => o.NormalizedUserName)
			.HasColumnName("normalized_user_name");

			config.Property(o => o.ConcurrencyStamp)
			.HasColumnName("concurrency_stamp");

			config.Property(o => o.LockoutEnd)
			.HasColumnName("lockout_end");

			config.Property(o => o.FullName)
			.HasColumnName("full_name");

			config.HasMany<PublicUserClaim>(o => o.UserClaims)
				.WithOne(s => s.User)
				.HasForeignKey(s => s.UserId);

			config.HasMany<PublicUserLogin>(o => o.UserLogins)
				.WithOne(s => s.User)
				.HasForeignKey(s => s.UserId);

			config.HasMany<PublicUserRole>(o => o.UserRoles)
				.WithOne(s => s.User)
				.HasForeignKey(s => s.UserId);

	  }
	}
}