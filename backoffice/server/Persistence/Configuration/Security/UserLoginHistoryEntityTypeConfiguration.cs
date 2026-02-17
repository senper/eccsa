using ECCSA.BackOffice.Server.Model.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class UserLoginHistoryEntityTypeConfiguration :
		IEntityTypeConfiguration<UserLoginHistory>
	{
		public void Configure(EntityTypeBuilder<UserLoginHistory> config)
		{
			config.ToTable("user_login_history", "security");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.IPAddress)
			.HasColumnName("ip_address")
			.IsRequired()
			.HasMaxLength(50);

			config.Property(o => o.Date)
			.HasColumnName("date")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.Time)
			.HasColumnName("time")
			.IsRequired();

			config.Property(o => o.UserId)
			.HasColumnName("user_id")
			.HasColumnType("uuid")
			.IsRequired();

		}
	}
}