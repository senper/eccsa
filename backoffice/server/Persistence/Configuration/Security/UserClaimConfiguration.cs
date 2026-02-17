using ECCSA.BackOffice.Server.Model.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class UserClaimEntityTypeConfiguration:
		IEntityTypeConfiguration<UserClaim>
	{
	  public void Configure(EntityTypeBuilder<UserClaim> config)
	  {
			config.ToTable("user_claims", "security") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.UserId)
			.HasColumnName("user_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.ClaimType)
			.HasColumnName("claim_type")
			
			.IsRequired()
			
			.IsUnicode();


			config.Property(o => o.ClaimValue)
			.HasColumnName("claim_value")
			
			.IsRequired()
			
			.IsUnicode();




		   

	  }
	}
}