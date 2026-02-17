using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class NeedAssessmentEntityTypeConfiguration:
		IEntityTypeConfiguration<NeedAssessment>
	{
	  public void Configure(EntityTypeBuilder<NeedAssessment> config)
	  {
			config.ToTable("need_assessments", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.StartDate)
			.HasColumnName("start_date")
			.HasColumnType("date")
			.IsRequired();


			config.Property(o => o.EndDate)
			.HasColumnName("end_date")
			.HasColumnType("date")
			.IsRequired();


			config.Property(o => o.Remark)
			.HasColumnName("remark")
			
						
			.IsUnicode();


			config.Property(o => o.Status)
			.HasColumnName("status")
			.HasColumnType("int")
			.IsRequired();


			config.HasMany<NeedAssessmentDetail>(o => o.NeedAssessmentDetails)
				.WithOne(s => s.NeedAssessment)
				.HasForeignKey(s => s.AssessmentId);




		   

	  }
	}
}