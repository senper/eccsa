using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class BusinessProfileEntityTypeConfiguration:
        IEntityTypeConfiguration<BusinessProfile>
    {
      public void Configure(EntityTypeBuilder<BusinessProfile> config)
      {
            config.ToTable("business_profile", "membership") ;
            

            config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.IndividualId)
			.HasColumnName("individual_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.ProfileAttribureId)
			.HasColumnName("profile_attribure_id")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.AttributeValue)
			.HasColumnName("attribute_value")
			
						
			.IsUnicode();




           

      }
    }
}