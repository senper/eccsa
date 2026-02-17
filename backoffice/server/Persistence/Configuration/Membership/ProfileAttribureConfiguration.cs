using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class ProfileAttribureEntityTypeConfiguration:
		IEntityTypeConfiguration<ProfileAttribure>
	{
	  public void Configure(EntityTypeBuilder<ProfileAttribure> config)
	  {
			config.ToTable("profile_attribure", "membership") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("int")
			.IsRequired();


			
			

			config.Property(o => o.NameEng)
			.HasColumnName("name_eng")
			
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();


			config.Property(o => o.NameLocal)
			.HasColumnName("name_local")
			
			.IsRequired()
			
			.IsUnicode();

			config.Property(o => o.AttributeType)
			.HasColumnName("attribute_type")
			.IsRequired();

			config.Property(o => o.Max)
			.HasColumnName("attribute_max")
			.HasDefaultValue(0)
			.IsRequired();

			config.Property(o => o.Min)
			.HasColumnName("attribute_min")
			.HasDefaultValue(0)
			.IsRequired();

			config.Property(o => o.ForAssociation)
					.HasColumnName("for_association")
					.IsRequired();

			config.Property(o => o.ForIndividual)
				.HasColumnName("for_individual")
				.IsRequired();

			config.Property(o => o.LookupTypeId)
			.HasColumnName("lookup_type_id")
			.IsRequired(false);

			config.Property(o => o.Source)
			.HasColumnName("attribute_source")
			.HasColumnType("jsonb");

			config.HasMany<BusinessProfile>(o => o.BusinessProfiles)
				.WithOne(s => s.ProfileAttribure)
				.HasForeignKey(s => s.ProfileAttribureId);

			config.HasMany<AssociationProfile>(o => o.AssociationProfiles)
				.WithOne(s => s.ProfileAttribure)
				.HasForeignKey(s => s.ProfileAttribureId);








		}
	}
}