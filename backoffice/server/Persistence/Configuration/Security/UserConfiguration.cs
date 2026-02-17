using ECCSA.BackOffice.Server.Model.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class UserEntityTypeConfiguration :
        IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> config)
        {
            config.ToTable("users", "security");
            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();

            config.Property(o => o.AssociationId)
                .HasColumnName("association_id");

            config.Property(o => o.DepartmentId)
                .IsRequired(false)
                .HasColumnName("department_id");

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

            config.Property(o => o.AssignedModules)
                .HasColumnName("assigned_modules");

            config.Property(o => o.DefaultModule)
                .HasColumnName("default_module");


            config.Property(o => o.Disabled)
                .HasColumnName("disabled")
                .IsRequired()
                .HasDefaultValue(false);

            config.Property(o => o.TIN)
            .HasColumnName("tin")
            .HasMaxLength(50);

            config.Property(o => o.Address)
            .HasColumnName("address")
            .HasMaxLength(500);

            config.Property(o => o.MobileNo)
            .HasColumnName("mobile_no")
            .HasMaxLength(50);

            config.Property(o => o.MobileNoConfirmed)
            .HasColumnName("mobile_no_confirmed")
            .HasMaxLength(50);

            config.Property(o => o.MobileConfirmationCode)
            .HasColumnName("mobile_confirmation_code")
            .HasMaxLength(50);

            config.Property(o => o.ConfirmationCodeExpiryDate)
            .HasColumnName("confirmation_code_expiry_date");

            config.Property(o => o.PasswordResetCodeExpiryDate)
            .HasColumnName("password_reset_code_expiry_date");

            config.Property(o => o.PasswordResetCode)
            .HasColumnName("password_reset_code");

            config.Property(o => o.EmailConfirmationCode)
            .HasColumnName("email_confirmation_code")
            .HasMaxLength(50);

            config.Property(o => o.OldMobileNo)
            .HasColumnName("old_mobile_no")
            .HasMaxLength(50);

            config.Property(o => o.OldEmail)
            .HasColumnName("old_email")
            .HasMaxLength(50);

            config.Property(o => o.Title)
            .HasColumnName("title")
            .HasMaxLength(50);

            config.Property(o => o.CompanyName)
            .HasColumnName("company_name")
            .HasMaxLength(255);

            config.Property(o => o.BusinessAccount)
            .HasColumnName("business_account");

            config.Property(o => o.LogoExtension)
            .HasColumnName("logo_extension")
            .IsRequired(false);

            config.Property(o => o.PasswordTryCount)
            .HasColumnName("password_try_count");

            config.Property(o => o.PasswordDisabledTime)
            .HasColumnName("password_disabled_time")
            .IsRequired(false);

            config.HasMany<UserClaim>(o => o.UserClaims)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            config.HasMany<UserLogin>(o => o.UserLogins)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            config.HasMany<UserRole>(o => o.UserRoles)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

        }
    }

   
}