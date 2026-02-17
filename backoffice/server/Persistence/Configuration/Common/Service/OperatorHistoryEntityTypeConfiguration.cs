using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class OperatorHistoryEntityTypeConfiguration :
		IEntityTypeConfiguration<OperatorHistory>
	{
		public void Configure(EntityTypeBuilder<OperatorHistory> config)
		{
			config.ToTable("operator_history", "services");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd()
			.IsRequired();

			config.Property(o => o.ServiceRequestId)
					.HasColumnName("service_id")
					.IsRequired();

			config.Property(o => o.OperatorName)
					.HasColumnName("name")
					.HasMaxLength(50)
					.IsRequired();

			config.Property(o => o.UserId)
					.HasColumnName("userid")
					.IsRequired();

			config.Property(o => o.StartDate)
					.HasColumnName("start_date")
					.IsRequired();

			config.Property(o => o.EndDate)
					.HasColumnName("end_date")
					.IsRequired(false);

			



		}
	}
}