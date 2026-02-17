using ECCSA.BackOffice.Server.Model.PublicSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class PublicRoleEntityTypeConfiguration :
		IEntityTypeConfiguration<PublicRole>
	{
	  public void Configure(EntityTypeBuilder<PublicRole> config)
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


			config.HasMany<PublicUserRole>(o => o.UserRoles)
				.WithOne(s => s.Role)
				.HasForeignKey(s => s.RoleId);




		   

	  }
	}
}