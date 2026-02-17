using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class FlagEntityTypeConfiguration:
		IEntityTypeConfiguration<Flag>
	{
	  public void Configure(EntityTypeBuilder<Flag> config)
	  {
			config.ToTable("flags") ;
			

			config.HasKey(o => new { o.Id , o.FlagTypeId });


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.FlagTypeId)
			.HasColumnName("flag_type_id")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.Name)
			.HasColumnName("name")
			.HasColumnType("varchar")
						.HasMaxLength(50);


			config.Property(o => o.NameLocal)
			.HasColumnName("name_local")
			
						.HasMaxLength(1000)
			.IsUnicode();




		   

	  }
	}

}