using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class MembershipRequirementEntityTypeConfiguration :
		IEntityTypeConfiguration<MembershipRequirement>
	{
		public void Configure(EntityTypeBuilder<MembershipRequirement> config)
		{
			config.ToTable("membership_requirements", "membership");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.AssociationId)
			.HasColumnName("association_id")
			.IsRequired();

			config.Property(o => o.RequirementType)
				  .HasColumnName("requirement_type")
				  .IsRequired();

			config.Property(o => o.ApplicationRequirementLevel)
			.HasColumnName("application_requirement_level")
			.IsRequired();


			config.Property(o => o.RenewalRequirementLevel)
			.HasColumnName("renewal_requirement_level")
			.IsRequired();


			config.Property(o => o.OrderNo)
					.HasColumnName("order_no")
					.HasMaxLength(10)
					.IsRequired();

			config.Property(o => o.Name)
					.HasColumnName("name")
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode();

			

			config.Property(o => o.Label)
					.HasColumnName("label")
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode();


			config.Property(o => o.LabelLocal)
					.HasColumnName("label_local")
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode();

			config.Property(o => o.Description)
					.HasColumnName("description")
					.IsRequired()
					.HasMaxLength(800)
					.IsUnicode();

			
			config.Property(o => o.InputMethod)
					.HasColumnName("input_method")
					.IsRequired();



			config.Property(o => o.UpperLimit)
					.HasColumnName("upper_limit")
					.IsRequired();

			config.Property(o => o.LowerLimit)
					.HasColumnName("lower_limit")
					.IsRequired();

			config.Property(o => o.Status)
					.HasColumnName("status")
					.IsRequired();

			config.Property(o => o.ApproveOnSubmit)
					.HasColumnName("approve_on_submit")
					.IsRequired();


			config.Property(o => o.LookupTypeId)
						.HasColumnName("lookup_type_id")
						.IsRequired(false);


			config.HasMany<ApplicantDocument>(o => o.ApplicantDocuments)
			  .WithOne(s => s.Requirement)
			  .HasForeignKey(s => s.RequirementId);

			config.HasMany<LegalDocument>(o => o.IndividualDocuments)
			  .WithOne(s => s.Requirement)
			  .HasForeignKey(s => s.RequirementId);

		}
	}
}