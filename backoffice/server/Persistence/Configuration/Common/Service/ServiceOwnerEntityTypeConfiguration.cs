using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class ServiceOwnerEntityTypeConfiguration :
		IEntityTypeConfiguration<ServiceOwner>
	{
		public void Configure(EntityTypeBuilder<ServiceOwner> config)
		{
			config.ToTable("service_owners", "services");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.Title)
					.HasColumnName("title")
					.HasMaxLength(500)
					.IsRequired()
					.IsUnicode();

			config.Property(o => o.TitleLocal)
					.HasColumnName("title_local")
					.HasMaxLength(500)
					.IsRequired()
					.IsUnicode();

			config.Property(o => o.Description)
					.HasColumnName("description")
					.HasDefaultValue(string.Empty)
					//.HasMaxLength(500)
					.IsRequired()
					.IsUnicode();

			config.Property(o => o.DescriptionLocal)
					.HasColumnName("description_local")
					.HasDefaultValue(string.Empty)
					//.HasMaxLength(500)
					.IsRequired()
					.IsUnicode();

			config.Property(o => o.Status)
					.HasColumnName("status")
					.HasDefaultValue(StatusEnum.Active)
					.IsRequired();
		}
	}
}