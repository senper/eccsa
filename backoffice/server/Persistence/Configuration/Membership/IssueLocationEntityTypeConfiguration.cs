using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class IssueLocationEntityTypeConfiguration :
		IEntityTypeConfiguration<IssueLocation>
	{
		public void Configure(EntityTypeBuilder<IssueLocation> config)
		{
			config.ToTable("issue_location", "membership");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.Name)
			.HasColumnName("name")
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();

			config.Property(o => o.CountryRegion)
			.HasColumnName("country_region")
			.HasMaxLength(255)
			.IsRequired()
			.IsUnicode();

			config.Property(o => o.TypeOfLocationId)
			.HasColumnName("location_type_id")
			.IsRequired();

			config.HasMany<Issue>(o => o.Issues)
				.WithOne(s => s.Location)
				.HasForeignKey(s => s.LocationId);
		}
	}
}