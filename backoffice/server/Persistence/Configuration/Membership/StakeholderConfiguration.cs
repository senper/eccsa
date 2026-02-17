using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class StakeholderEntityTypeConfiguration:
		IEntityTypeConfiguration<Stakeholder>
	{
	  public void Configure(EntityTypeBuilder<Stakeholder> config)
	  {
			config.ToTable("stakeholders", "membership") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.StakeholderType)
			.HasColumnName("stakeholder_type")
			.IsRequired();


			config.Property(o => o.Status)
			.HasColumnName("status")
			.IsRequired();

			config.HasOne<Individual>(o => o.Individual)
				.WithOne(o => o.Stakeholder)
				.HasForeignKey<Individual>(s => s.Id);

			config.HasOne<Association>(o => o.Association)
				.WithOne(o => o.Stakeholder)
				.HasForeignKey<Association>(s => s.Id);

			config.HasMany<Address>(o => o.Addresses)
				.WithOne(s => s.Stakeholder)
				.HasForeignKey(s => s.MemberId);

			config.HasMany<Contact>(o => o.Contacts)
				.WithOne(s => s.Stakeholder)
				.HasForeignKey(s => s.MemberId);


			config.HasMany<MembershipEnrollment>(o => o.MembershipEnrollments)
				.WithOne(s => s.Stakeholder)
				.HasForeignKey(s => s.MemberId);


			config.HasMany<Representative>(o => o.Representatives)
				.WithOne(s => s.Stakeholder)
				.HasForeignKey(s => s.MemberId);




		   

	  }
	}
}