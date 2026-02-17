using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class ContactUsEntityTypeConfiguration :
		IEntityTypeConfiguration<ContactUs>
	{
		public void Configure(EntityTypeBuilder<ContactUs> config)
		{
			config.ToTable("contact_us", "membership");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasMaxLength(255)
			.IsRequired();

			config.Property(o => o.Name)
			.HasColumnName("name")
			.IsRequired()
			.IsUnicode();

			config.Property(o => o.Email)
			.HasColumnName("email")
			.IsRequired()
			.HasMaxLength(255)
			.IsUnicode();

			config.Property(o => o.Message)
			.HasColumnName("message")
			.IsRequired()
			.HasMaxLength(1000)
			.IsUnicode();

			config.Property(o => o.Date)
			.HasColumnName("date")
			.IsRequired();
		}
	}
}