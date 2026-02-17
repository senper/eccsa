using ECCSA.BackOffice.Server.Model.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Communication
{
	public class MediaCoverageEntityTypeConfiguration:
		IEntityTypeConfiguration<MediaCoverage>
	{
	  public void Configure(EntityTypeBuilder<MediaCoverage> config)
	  {
			config.ToTable("media_coverages", "communication") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired()
			.ValueGeneratedOnAdd();


			config.Property(o => o.MediaAgencyId)
			.HasColumnName("media_agency_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.StartDate)
			.HasColumnName("start_date")
			;
			

			config.Property(o => o.EndDate)
			.HasColumnName("end_date")
			;
			

			config.Property(o => o.DurationMinutes)
			.HasColumnName("duration_minutes")
			.HasColumnType("int");
			

			config.Property(o => o.Frequency)
			.HasColumnName("frequency")
			.HasMaxLength(50);
			

			config.Property(o => o.CoveredIn)
			.HasColumnName("covered_in")
			.HasMaxLength(50);
			

			config.Property(o => o.About)
			.HasColumnName("about")
			
						.HasMaxLength(300)
			.IsUnicode();

            config.Property(o => o.Extension)
                .HasColumnName("extension")
                .HasDefaultValue("")
                .HasMaxLength(10)
                .IsRequired()
                .IsUnicode();


            config.Property(o => o.Remark)
			.HasColumnName("remark")
			
						.HasMaxLength(2000)
			.IsUnicode();


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