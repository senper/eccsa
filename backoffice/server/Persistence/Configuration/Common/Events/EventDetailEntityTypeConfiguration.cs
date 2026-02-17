using ECCSA.BackOffice.Server.Model.Common.EventManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Common.Events
{
    public class EventDetailEntityTypeConfiguration :
		IEntityTypeConfiguration<EventDetail>
	{
		public void Configure(EntityTypeBuilder<EventDetail> config)
		{
			config.ToTable("event_details", "eventmanagement");

			config.HasKey(o => o.DetailId);

			config.Property(o => o.DetailId)
				.HasColumnName("detail_id")
				.IsRequired();

			config.Property(o => o.EventId)
				.HasColumnName("event_id")
				.IsRequired();
			
			config.Property(o => o.Title)
					.HasColumnName("title")
					.HasMaxLength(100)
					.IsRequired()
					.IsUnicode();

            config.Property(o => o.TitleLocal)
                    .HasColumnName("title_local")
                    .HasMaxLength(100)
                    .IsRequired(false)
                    .IsUnicode();

            config.Property(o => o.ServiceTypeId)
					.HasColumnName("servie_type_id")
					.IsRequired(false);

			config.Property(o => o.StartDate)
					.HasColumnName("start_date")
					.HasColumnType("date")
					.IsRequired();
			config.Property(o => o.EndDate)
					.HasColumnName("end_date")
					.HasColumnType("date")
					.IsRequired();

		}
	}
}