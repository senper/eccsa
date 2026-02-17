using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class IssueAttachmentEntityTypeConfiguration :
		IEntityTypeConfiguration<IssueAttachment>
	{
		public void Configure(EntityTypeBuilder<IssueAttachment> config)
		{
			config.ToTable("issue_attachments", "membership");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.IssueId)
			.HasColumnName("issue_id")
			.IsRequired();

			config.Property(o => o.Title)
			.HasColumnName("title")
			.IsRequired()
			.HasMaxLength(255)
			.IsUnicode();

			config.Property(o => o.Extension)
			.HasColumnName("extension")
			.IsRequired()
			.HasMaxLength(10);
		}
	}
}