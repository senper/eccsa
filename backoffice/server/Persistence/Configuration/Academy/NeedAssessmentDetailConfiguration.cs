using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class NeedAssessmentDetailEntityTypeConfiguration:
		IEntityTypeConfiguration<NeedAssessmentDetail>
	{
	  public void Configure(EntityTypeBuilder<NeedAssessmentDetail> config)
	  {
			config.ToTable("need_assessment_details", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.CategoryId)
			.HasColumnName("category_id")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.MemberId)
			.HasColumnName("member_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.AssessmentId)
			.HasColumnName("assessment_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.NeedRating)
			.HasColumnName("need_rating")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.BestBefore)
			.HasColumnName("best_before")
			.HasColumnType("date")
			.IsRequired();


			config.Property(o => o.Remark)
			.HasColumnName("remark")
			
						
			.IsUnicode();


			config.Property(o => o.Status)
			.HasColumnName("status")
			.HasColumnType("int")
			.IsRequired();




		   

	  }
	}
}