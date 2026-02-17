using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class AssessmentEntityTypeConfiguration:
		IEntityTypeConfiguration<Assessment>
	{
	  public void Configure(EntityTypeBuilder<Assessment> config)
	  {
			config.ToTable("assessments", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.ValueGeneratedOnAdd()
			.IsRequired();


			config.Property(o => o.TrainingId)
			.HasColumnName("training_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.RegistrationId)
			.HasColumnName("registration_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.Date)
			.HasColumnName("date")
			.IsRequired();


			config.Property(o => o.ResultScore)
			.HasColumnName("result_score")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.ResultPass)
			.HasColumnName("result_pass")
			.HasColumnType("int")
			.IsRequired();

			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o=> o.CreateStamp)
				.HasColumnName("create_stamp")
				.HasDefaultValue(DateTime.UtcNow)
				.IsRequired();

			config.Property(o => o.UpdatedBy)
				.HasColumnName("updated_by")
				.IsRequired();

			config.Property(o => o.UpdateStamp)
				.HasColumnName("update_stamp")
				.HasDefaultValue(DateTime.UtcNow)
				.IsRequired();


		   

	  }
	}
}