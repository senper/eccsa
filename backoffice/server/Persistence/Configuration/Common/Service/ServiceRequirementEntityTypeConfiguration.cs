using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class ServiceRequirementEntityTypeConfiguration :
    IEntityTypeConfiguration<ServiceRequirement>
    {
        public void Configure(EntityTypeBuilder<ServiceRequirement> config)
        {
            config.ToTable("service_requirements", "services");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();


            config.Property(o => o.ServiceTypeId)
                    .HasColumnName("service_type_id")
                    .IsRequired();
            config.Property(o => o.RequirementType)
                  .HasColumnName("requirement_type")
                  .IsRequired();

            config.Property(o => o.ParentId)
                  .HasColumnName("parent_id")
                  .IsRequired(false);

            config.Property(o => o.OrderNo)
                    .HasColumnName("order_no")
                    .HasMaxLength(10)
                    .IsRequired();

            config.Property(o => o.NameEng)
                    .HasColumnName("name_eng")
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode();

            config.Property(o => o.NameLocal)
                    .HasColumnName("name_local")
                    .HasMaxLength(100)
                    .IsUnicode();

            config.Property(o => o.Label)
                    .HasColumnName("label")
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode();

            config.Property(o => o.LabelLocal)
                    .HasColumnName("label_local")
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode();

            config.Property(o => o.DescriptionEng)
                    .HasColumnName("description_eng")
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode();

            config.Property(o => o.DescriptionLocal)
                    .HasColumnName("description_local")
                    .IsRequired()
                    .IsUnicode();
            config.Property(o => o.InputMethod)
                    .HasColumnName("input_method")
                    .IsRequired();



            config.Property(o => o.UpperLimit)
                    .HasColumnName("upper_limit")
                    .IsRequired();

            config.Property(o => o.LowerLimit)
                    .HasColumnName("lower_limit")
                    .IsRequired();

            config.Property(o => o.Status)
                    .HasColumnName("status")
                    .IsRequired();

            config.Property(o => o.ApproveOnSubmit)
                    .HasColumnName("approve_on_submit")
                    .IsRequired();

            config.Property(o => o.IsForOpponent)
                    .HasColumnName("is_for_opponent")
                    .HasDefaultValue(false)
                    .IsRequired();


            config.Property(o => o.LookupTypeId)
                        .HasColumnName("lookup_type_id")
                        .IsRequired(false);

            config.HasMany<ServiceTypeReport>(o => o.ServiceTypeReports)
                .WithOne(s => s.ServiceRequirement)
                .HasForeignKey(s => s.ServiceRequirementId);
        }
    }
}