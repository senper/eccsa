using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class IndividualServicesEntityTypeConfiguration :
		IEntityTypeConfiguration<IndividualService>
	{
		public void Configure(EntityTypeBuilder<IndividualService> config)
		{
			config.ToTable("individual_services", "membership");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.IndividualId)
			.HasColumnName("individual_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.SerNo)
			.HasColumnName("ser_no")
			.HasMaxLength(10)
			.IsRequired();


			config.Property(o => o.ServiceName)
			.HasColumnName("service_name")
			.HasMaxLength(250)
			.IsRequired()
			.IsUnicode();






		}
	}
}