using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class SettingVariableEntityTypeConfiguration:
		IEntityTypeConfiguration<SettingVariable>
	{
	  public void Configure(EntityTypeBuilder<SettingVariable> config)
	  {
			config.ToTable("setting_variables", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("int")
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


			config.Property(o => o.DataType)
			.HasColumnName("data_type")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.MinValue)
			.HasColumnName("min_value")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.MaxValue)
			.HasColumnName("max_value")
			.HasColumnType("int")
			.IsRequired();


			config.HasMany<TopicSetting>(o => o.TopicSettings)
				.WithOne(s => s.SettingVariable)
				.HasForeignKey(s => s.SettingVariableId);




		   

	  }
	}
}