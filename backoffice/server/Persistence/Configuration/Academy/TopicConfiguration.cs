using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class TopicEntityTypeConfiguration:
		IEntityTypeConfiguration<Topic>
	{
	  public void Configure(EntityTypeBuilder<Topic> config)
	  {
			config.ToTable("topics", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
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


			config.Property(o => o.CategoryId)
			.HasColumnName("category_id")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.Decription)
				.HasColumnName("decription")
				.IsUnicode();


			config.Property(o => o.DecriptionLocal)
			.HasColumnName("decription_local")
			.IsUnicode();

			config.Property(o => o.IsPublic)
				.HasColumnName("is_public")
				.IsRequired();

			config.Property(o => o.Status)
			.HasColumnName("status")
			.IsRequired();

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
			config.HasMany<LearningResource>(o => o.Resources)
				.WithOne(s => s.Topic)
				.HasForeignKey(s => s.TopicId);


			config.HasMany<TopicSetting>(o => o.TopicSettings)
				.WithOne(s => s.Topic)
				.HasForeignKey(s => s.TopicId);


			config.HasMany<Training>(o => o.Trainings)
				.WithOne(s => s.Topic)
				.HasForeignKey(s => s.TopicId);




		   

	  }
	}
}