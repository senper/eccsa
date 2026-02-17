using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class MemberEntityTypeConfiguration:
		IEntityTypeConfiguration<Member>
	{
	  public void Configure(EntityTypeBuilder<Member> config)
	  {
			config.ToTable("members", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
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


			config.Property(o => o.MemberType)
			.HasColumnName("member_type")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.MembershipId)
			.HasColumnName("membership_id")
			.HasColumnType("uuid");
			

			config.Property(o => o.AddressType)
			.HasColumnName("address_type")
			.HasColumnType("int");
			

			config.Property(o => o.LocationId)
			.HasColumnName("location_id")
			.HasColumnType("uuid");
			

			config.Property(o => o.AddressLine)
			.HasColumnName("address_line")
			
						.HasMaxLength(500)
			.IsUnicode();


			config.Property(o => o.ContactType)
			.HasColumnName("contact_type")
			.HasColumnType("int");
			

			config.Property(o => o.Contact)
			.HasColumnName("contact")
			
						.HasMaxLength(500)
			.IsUnicode();


			config.HasMany<NeedAssessmentDetail>(o => o.NeedAssessmentDetails)
				.WithOne(s => s.Member)
				.HasForeignKey(s => s.MemberId);




		   

	  }
	}
}