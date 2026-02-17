using ECCSA.BackOffice.Server.Model.Common.EventManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Common.Events
{

	public class EventTypeEntityTypeConfiguration :
		IEntityTypeConfiguration<EventType>
	{
		public void Configure(EntityTypeBuilder<EventType> config)
		{
			config.ToTable("event_types", "eventmanagement");

			config.HasKey(o => o.EventTypeId);

			config.Property(o => o.EventTypeId)
				.HasColumnName("event_type_id")
				.IsRequired();



			config.Property(o => o.Title)
					.HasColumnName("title")
					.HasMaxLength(100)
					.IsRequired()
					.IsUnicode();

			config.Property(o => o.Description)
					.HasColumnName("description")
					.HasMaxLength(800)
					.IsRequired()
					.IsUnicode();
			config.Property(o => o.Status)
					.HasColumnName("status")
					.IsRequired();




			config.HasMany<Event>(o => o.Events)
				.WithOne(s => s.EventType)
				.HasForeignKey(s => s.EventTypeId);



		}
	}
	public class EventEntityTypeConfiguration :
		IEntityTypeConfiguration<Event>
	{
		public  void Configure(EntityTypeBuilder<Event> config)
		{
			config.ToTable("events","eventmanagement");

			 config.HasKey(o => o.Id);

			config.Property(o => o.Id)
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

            config.Property(o => o.Description)
					.HasColumnName("description")
					.HasMaxLength(800)
					.IsRequired()
					.IsUnicode();

            config.Property(o => o.DescriptionLocal)
                    .HasColumnName("description_local")
                    .HasMaxLength(800)
                    .IsRequired(false)
                    .IsUnicode();

            config.Property(o => o.EventTypeId)
					.HasColumnName("event_type_id")
					.IsRequired();

			config.Property(o => o.EventOwner)
					.HasColumnName("owner")
					.IsRequired();

			config.Property(o => o.StartDate)
					.HasColumnName("start_date")
					.HasColumnType("date")
					.IsRequired();

			config.Property(o => o.EndDate)
					.HasColumnName("end_date")
					.HasColumnType("date")
					.IsRequired();

			

			config.HasMany<EventDetail>(o => o.Details)
				.WithOne(s => s.Event)
				.HasForeignKey(s => s.EventId);



		}
	}
}