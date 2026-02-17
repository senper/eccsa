using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class AssociationProfileEntityTypeConfiguration :
		IEntityTypeConfiguration<AssociationProfile>
	{
		public void Configure(EntityTypeBuilder<AssociationProfile> config)
		{
			config.ToTable("association_profile", "membership");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.AssociationId)
			.HasColumnName("association_id")
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