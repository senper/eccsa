using ECCSA.BackOffice.Server.Model.Arbitration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Arbitration
{
	public class ArbitrationFollowupEntityTypeConfiguration :
			IEntityTypeConfiguration<ArbitrationFollowup>
	{
		public void Configure(EntityTypeBuilder<ArbitrationFollowup> config)
		{
			config.ToTable("followups", "arbitration");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd()
			.IsRequired();

			config.Property(o => o.ServiceId)
				.HasColumnName("service_id")
				.IsRequired();

			config.Property(o => o.TaskTypeId)
				.HasColumnName("task_type_id");

			config.Property(o => o.FollowupResultId)
				.HasColumnName("followup_result_id");


			config.Property(o => o.PlannedDate)
					.HasColumnName("planned_date")
					.HasColumnType("date");

			config.Property(o => o.Description)
					.HasColumnName("description")
					.HasMaxLength(500)
					.IsUnicode();
			config.Property(o => o.Remark)
					.HasColumnName("remark")
					.IsUnicode()
					.HasMaxLength(500);

			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o=> o.CreateStamp)
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