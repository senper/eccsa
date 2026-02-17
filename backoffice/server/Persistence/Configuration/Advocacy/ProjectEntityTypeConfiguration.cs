using ECCSA.BackOffice.Server.Model.Advocacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Advocacy
{
    public class ProjectEntityTypeConfiguration :
		IEntityTypeConfiguration<Project>
	{
		public void Configure(EntityTypeBuilder<Project> config)
		{
			config.ToTable("projects", "advocacy");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.Title)
			.HasColumnName("title")
			.HasColumnType("varchar")
			.HasMaxLength(100);

			config.Property(o => o.Description)
			.HasColumnName("description")
			.HasMaxLength(800)
			.IsUnicode();


			config.Property(o => o.PlannedStartDate)
			.HasColumnName("planned_start_date")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.PlannedEndDate)
			.HasColumnName("planned_end_date")
			.HasColumnType("date")
			.IsRequired();


			config.Property(o => o.StartDate)
			.HasColumnName("start_date")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.EndDate)
			.HasColumnName("end_date")
			.HasColumnType("date")
			.IsRequired(false);



			config.Property(o => o.Status)
				.HasColumnName("status");

			config.Property(o => o.Remark)
			.HasColumnName("remark")
			.HasMaxLength(100)
			.IsUnicode();

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
			config.HasMany<ProjectFollowup>(o => o.Followups)
				.WithOne(s => s.Project)
				.HasForeignKey(s => s.ProjectId);
		}
	}
}