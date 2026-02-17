using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class TopicSettingEntityTypeConfiguration:
		IEntityTypeConfiguration<TopicSetting>
	{
	  public void Configure(EntityTypeBuilder<TopicSetting> config)
	  {
			config.ToTable("topic_settings", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.TopicId)
			.HasColumnName("topic_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.SettingVariableId)
			.HasColumnName("setting_variable_id")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.SettingValue)
			.HasColumnName("setting_value")
			
			.IsRequired()
			
			.IsUnicode();

			


		   

	  }
	}
}