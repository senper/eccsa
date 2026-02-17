using ECCSA.BackOffice.Server.Model.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class UserRoleEntityTypeConfiguration:
		IEntityTypeConfiguration<UserRole>
	{
	  public void Configure(EntityTypeBuilder<UserRole> config)
	  {
			config.ToTable("user_roles", "security") ;
			

			config.HasKey(o => new { o.UserId , o.RoleId });


			config.Property(o => o.UserId)
			.HasColumnName("user_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.RoleId)
			.HasColumnName("role_id")
			.HasColumnType("uuid")
			.IsRequired();




		   

	  }
	}
}