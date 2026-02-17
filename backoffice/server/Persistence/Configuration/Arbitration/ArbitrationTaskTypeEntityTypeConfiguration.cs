using ECCSA.BackOffice.Server.Model.Arbitration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Arbitration
{
    public class ArbitrationTaskTypeEntityTypeConfiguration :
			IEntityTypeConfiguration<ArbitrationTaskType>
	{
		public void Configure(EntityTypeBuilder<ArbitrationTaskType> config)
		{
			config.ToTable("task_types", "arbitration");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd()
			.IsRequired();



			config.Property(o => o.NameEng)
					.HasColumnName("name_eng")
					.HasMaxLength(50);

			config.Property(o => o.NameLocal)
					.HasColumnName("name_local")
					.IsUnicode()
					.HasMaxLength(50);

			config.Property(o => o.NameEng)
					.HasColumnName("name_eng")
					.HasMaxLength(50);

			config.Property(o => o.DescriptionEng)
					.HasColumnName("description_eng")
					.HasMaxLength(500)
					.IsUnicode();

			config.Property(o => o.DescriptionLocal)
					.HasColumnName("description_local")
					.HasMaxLength(500)
					.IsUnicode();

			config.HasMany<ArbitrationFollowup>(o => o.Services)
				.WithOne(s => s.TaskType)
				.HasForeignKey(s => s.TaskTypeId);
		}
	}
}