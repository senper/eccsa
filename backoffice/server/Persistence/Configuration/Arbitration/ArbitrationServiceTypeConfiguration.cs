using ECCSA.BackOffice.Server.Model.Arbitration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Arbitration
{
	public class ArbitrationServiceTypeEntityTypeConfiguration:
		IEntityTypeConfiguration<ArbitrationServiceType>
	{
	  public void Configure(EntityTypeBuilder<ArbitrationServiceType> config)
	  {
			config.ToTable("arbitration_service_types", "arbitration") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd()
			.IsRequired();


			config.Property(o => o.NameEng)
				.HasColumnName("name_eng")
				.HasColumnType("varchar")
				.HasMaxLength(100);


			config.Property(o => o.NameLocal)
					.HasColumnName("name_local")
					.HasMaxLength(1000)
					.IsUnicode();


			config.Property(o => o.DescriptionEng)
					.HasColumnName("description_eng")
					.IsUnicode();

			config.Property(o => o.DescriptionLocal)
					.HasColumnName("description_local")
					.IsUnicode();

			config.Property(o => o.ServiceCharge)
					.HasColumnName("service_charge")
					.HasDefaultValue(0);
			

			config.Property(o => o.RequiresThirdParty)
				.HasColumnName("requires_third_party")
				.HasDefaultValue(Model.Common.YesNoEnum.NotSet);
			
			config.HasMany<ArbitrationService>(o => o.Services)
				.WithOne(s => s.ServiceType)
				.HasForeignKey(s => s.ServiceTypeId);


	  }
	}
}