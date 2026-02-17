using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class TrainingSponsorEntityTypeConfiguration:
		IEntityTypeConfiguration<TrainingSponsor>
	{
	  public void Configure(EntityTypeBuilder<TrainingSponsor> config)
	  {
			config.ToTable("training_sponsors", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.TrainingId)
			.HasColumnName("training_id")
			.HasColumnType("uuid");

			config.Property(o => o.ReferenceNo)
			.HasColumnName("reference_no");

			config.Property(o => o.Attachment)
			.HasColumnName("attachment");


			config.Property(o => o.SponsorId)
			.HasColumnName("sponsor_id")
			.HasColumnType("uuid");
			

			config.Property(o => o.Status)
			.HasColumnName("status")
			.HasColumnType("int");
			

			config.HasMany<Registration>(o => o.Registrations)
				.WithOne(s => s.TrainingSponsor)
				.HasForeignKey(s => s.TrainingSponsorId);




		   

	  }
	}
}