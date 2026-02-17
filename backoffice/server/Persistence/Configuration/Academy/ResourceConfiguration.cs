using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class LearningResourceEntityTypeConfiguration:
		IEntityTypeConfiguration<LearningResource>
	{
	  public void Configure(EntityTypeBuilder<LearningResource> config)
	  {
			config.ToTable("resources", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.TopicId)
			.HasColumnName("topic_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.Name)
			.HasColumnName("name")
			
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();


			config.Property(o => o.Description)
			.HasColumnName("description")
			
			.IsRequired()
			
			.IsUnicode();


			config.Property(o => o.ResourceType)
			.HasColumnName("resource_type")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.ResourceContent)
			.HasColumnName("resource")
			
						
			.IsUnicode();


			config.Property(o => o.Status)
			.HasColumnName("status")
			.HasColumnType("int")
			.IsRequired();


			config.HasMany<ResourceDelivery>(o => o.ResourceDeliveries)
				.WithOne(s => s.Resource)
				.HasForeignKey(s => s.ResourceId);




		   

	  }
	}
}