using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class CategoryEntityTypeConfiguration:
		IEntityTypeConfiguration<Category>
	{
	  public void Configure(EntityTypeBuilder<Category> config)
	  {
			config.ToTable("categories", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("int")
			//.ValueGeneratedOnAdd()
			.IsRequired();


			config.Property(o => o.Code)
			.HasColumnName("code")
			
			.IsRequired()
			.HasMaxLength(50)
			.IsUnicode();


			config.Property(o => o.NameEng)
			.HasColumnName("name_eng")
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();


			config.Property(o => o.NameLocal)
			.HasColumnName("name_local")
			
			.IsRequired()
			
			.IsUnicode();


			config.Property(o => o.ParentId)
			.HasColumnName("parent_id")
			.HasColumnType("int");
			

			config.Property(o => o.Status)
			.HasColumnName("status")
			.HasColumnType("int")
			.IsRequired();


			config.HasMany<Category>(o => o.ChildrenCategories)
				.WithOne(s => s.ParentCategory)
				.HasForeignKey(s => s.ParentId);


			config.HasMany<NeedAssessmentDetail>(o => o.NeedAssessmentDetails)
				.WithOne(s => s.Category)
				.HasForeignKey(s => s.CategoryId);


			config.HasMany<Topic>(o => o.Topics)
				.WithOne(s => s.Category)
				.HasForeignKey(s => s.CategoryId);




		   

	  }
	}
}