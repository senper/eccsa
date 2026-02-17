using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class ServiceTypeReportEntityTypeConfiguration :
        IEntityTypeConfiguration<ServiceTypeReport>
    {
        public void Configure(EntityTypeBuilder<ServiceTypeReport> config)
        {
            config.ToTable("service_types_reports", "services");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
            .HasColumnName("id")
            .IsRequired();

            config.Property(o => o.ServiceTypeId)
            .HasColumnName("service_type_id")
            .IsRequired();

            config.Property(o => o.ServiceRequirementId)
            .HasColumnName("requirement_id")
            .IsRequired();

            config.Property(o => o.Label)
                    .HasColumnName("label")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.FieldName)
                    .HasColumnName("field_name")
                    .HasMaxLength(255);

            config.Property(o => o.EnumValue)
                    .HasColumnName("enum_value")
                    .HasMaxLength(255);
        }
    }
}