using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class IssueTypeEntityTypeConfiguration :
		IEntityTypeConfiguration<IssueType>
	{
		public void Configure(EntityTypeBuilder<IssueType> config)
		{
			config.ToTable("issue_types", "membership");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.Code)
				.HasColumnName("code")
				.IsRequired()
				.HasMaxLength(20)
				.IsUnicode();

			config.Property(o => o.NameEng)
				.HasColumnName("name_eng")
				.IsRequired()
				.HasMaxLength(200)
				.IsUnicode();

			config.Property(o => o.NameLocal)
				.HasColumnName("name_local")
				.HasMaxLength(200)
				.IsRequired()
				.IsUnicode();

			config.Property(o => o.Status)
			.HasColumnName("status")
			.IsRequired();

			config.HasMany<Issue>(o => o.Issues)
				.WithOne(s => s.IssueType)
				.HasForeignKey(s => s.TypeId);
		}
	}
}