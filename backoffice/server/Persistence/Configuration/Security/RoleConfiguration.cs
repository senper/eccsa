using ECCSA.BackOffice.Server.Model.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class RoleEntityTypeConfiguration:
		IEntityTypeConfiguration<Role>
	{
	  public void Configure(EntityTypeBuilder<Role> config)
	  {
			config.ToTable("roles", "security") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.Name)
			.HasColumnName("name")
			
			.IsRequired()
			.HasMaxLength(250)
			.IsUnicode();

			config.Property(o => o.Module)
				.HasColumnName("module")
				.IsRequired();

			config.HasMany<UserRole>(o => o.UserRoles)
				.WithOne(s => s.Role)
				.HasForeignKey(s => s.RoleId);




		   

	  }
	}
}