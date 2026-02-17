using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class FlagTypeEntityTypeConfiguration:
        IEntityTypeConfiguration<FlagType>
    {
      public void Configure(EntityTypeBuilder<FlagType> config)
      {
            config.ToTable("flag_types") ;
            

            config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.Name)
			.HasColumnName("flag_type")
			.HasColumnType("varchar")
						.HasMaxLength(50);


			config.Property(o => o.NameLocal)
			.HasColumnName("flag_type_local")
			
						.HasMaxLength(1000)
			.IsUnicode();


			config.HasMany<Flag>(o => o.Flags)
				.WithOne(s => s.FlagType)
				.HasForeignKey(s => s.FlagTypeId);




           

      }
    }
}