using ECCSA.BackOffice.Server.Model.PublicSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class PublicUserRoleEntityTypeConfiguration :
		IEntityTypeConfiguration<PublicUserRole>
	{
	  public void Configure(EntityTypeBuilder<PublicUserRole> config)
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