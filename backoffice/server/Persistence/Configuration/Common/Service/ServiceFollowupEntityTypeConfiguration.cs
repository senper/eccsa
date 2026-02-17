using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class ServiceFollowupEntityTypeConfiguration :
		IEntityTypeConfiguration<ServiceFollowup>
	{
		public void Configure(EntityTypeBuilder<ServiceFollowup> config)
		{
			config.ToTable("service_followups","services");

			 config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd()
			.IsRequired();

			config.Property(o => o.ServiceId)
					.HasColumnName("service_id")
					.IsRequired();

			config.Property(o => o.ServiceTypeId)
					.HasColumnName("service_type_id")
					.IsRequired();

			config.Property(o => o.Title)
					.HasColumnName("title")
					.HasMaxLength(100)
					.IsRequired();


			config.Property(o => o.StartDate)
					.HasColumnName("start_date")
					.HasColumnType("date")
					.IsRequired();

			config.Property(o => o.StartTime)
					.HasColumnName("start_time")
					.HasMaxLength(4)
					.IsRequired();

		

			config.Property(o => o.Description)
					.HasColumnName("description")
					.IsRequired()
					.IsUnicode();

			

		

			config.Property(o => o.Extension)
					.HasColumnName("extension")
					.HasMaxLength(10)
					.IsRequired(false);

			config.Property(o => o.Status)
					.HasColumnName("status")
					.IsRequired();
	
			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o => o.CreateStamp)
				.HasColumnName("create_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();

			config.Property(o => o.UpdatedBy)
				.HasColumnName("updated_by")
				.IsRequired();

			config.Property(o => o.UpdateStamp)
				.HasColumnName("update_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();



		}
	}
}