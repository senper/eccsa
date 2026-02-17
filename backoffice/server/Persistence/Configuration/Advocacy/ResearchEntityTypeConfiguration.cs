using ECCSA.BackOffice.Server.Model.Advocacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Advocacy
{
    public class ResearchEntityTypeConfiguration :
			IEntityTypeConfiguration<Research>
	{
		public void Configure(EntityTypeBuilder<Research> config)
		{
			config.ToTable("researches", "advocacy");


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


			config.Property(o => o.ConductedBy)
			.HasColumnName("conducted_by")
			.HasMaxLength(100)
			.IsUnicode();


			config.Property(o => o.Target)
			.HasColumnName("target")
			.HasMaxLength(100)
			.IsUnicode();

			config.Property(o => o.Finding)
			.HasColumnName("finding")
			.HasMaxLength(800)
			.IsUnicode();

			config.Property(o => o.Stage)
			.HasColumnName("stage");

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

		}
	}
}