using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class AttendanceEntityTypeConfiguration:
		IEntityTypeConfiguration<Attendance>
	{
	  public void Configure(EntityTypeBuilder<Attendance> config)
	  {
			config.ToTable("attendances", "training") ;
			

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


			config.Property(o => o.AttendanceDate)
			.HasColumnName("attendance_date")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.Morning)
					.HasColumnName("morning_training")
					.HasDefaultValue(false);

			config.Property(o => o.Afternoon)
					.HasColumnName("afternoon_training")
					.HasDefaultValue(false);

			config.Property(o => o.Evening)
					.HasColumnName("evening_training")
					.HasDefaultValue(false);
		}
	}
}