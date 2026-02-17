using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class ServiceTypeEntityTypeConfiguration :
		IEntityTypeConfiguration<ServiceType>
	{
		public  void Configure(EntityTypeBuilder<ServiceType> config)
		{
			config.ToTable("service_types", "services");

			 config.HasKey(o => o.Id);

			config.Property(o => o.NameEng)
				.HasColumnName("name_eng")
				.IsRequired()
				.HasMaxLength(100);


			config.Property(o => o.NameLocal)
					.HasColumnName("name_local")
					.HasMaxLength(1000)
					.IsUnicode();
			config.Property(o => o.CertificateType)
					.HasColumnName("certificate_type")
					.IsRequired();

			config.Property(o => o.DescriptionEng)
					.HasColumnName("description_eng")
					.IsUnicode();

			config.Property(o => o.ApprovalRequired)
					.HasColumnName("approval_required")
					.IsRequired();

			config.Property(o => o.IsResourceServiceType)
					.HasColumnName("resource_service_type")
					.IsRequired();

            config.Property(o => o.IsArbitration)
                    .HasColumnName("is_arbitration")
					.HasDefaultValue(false)
                    .IsRequired();

            config.Property(o => o.AllowOptionalEntries)
                    .HasColumnName("allow_optional_entries")
					.HasDefaultValue(false)
                    .IsRequired();

            config.Property(o => o.AllowAdditionalAttachments)
					.HasColumnName("allow_attachments")
					.IsRequired();
			config.Property(o => o.ApprovalAttachmentRequired)
					.HasColumnName("attachment_required")
					.IsRequired();
			
			config.Property(o => o.DescriptionLocal)
					.HasColumnName("description_local")
					.IsUnicode();

			config.Property(o => o.MemberServiceCharge)
					.HasColumnName("member_service_charge")
					.IsRequired()
					.HasDefaultValue(0);

			config.Property(o => o.NonMemberServiceCharge)
					.HasColumnName("public_service_charge")
					.IsRequired()
					.HasDefaultValue(0);
			config.Property(o => o.RateBasis)
				.HasColumnName("rate_basis")
				.IsRequired();

			config.Property(o => o.Status)
				.HasColumnName("status")
				.IsRequired();

			config.Property(o => o.StartDate)
				.HasColumnName("start_date")
				.HasColumnType("date")
				.IsRequired(false);

			config.Property(o => o.EndDate)
				.HasColumnName("end_date")
				.HasColumnType("date")
				.IsRequired(false);

			config.Property(o => o.ReportTemplate)
				.HasColumnName("report_template")
				.HasMaxLength(255)
				.IsRequired(false);

			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o => o.CreateStamp)
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
            
            config.Property(o => o.WorkerList)
                .HasColumnName("worker_list")
                .HasColumnType("jsonb")
                .IsRequired(false);

            config.HasMany<ServiceRequest>(o => o.ServiceRequests)
				.WithOne(s => s.ServiceType)
				.HasForeignKey(s => s.ServiceTypeId);

			config.HasMany<ServiceRequirement>(o => o.ServiceRequirements)
				.WithOne(s => s.ServiceType)
				.HasForeignKey(s => s.ServiceTypeId);

			config.HasMany<ServiceTypeReport>(o => o.ServiceTypeReports)
				.WithOne(s => s.ServiceType)
				.HasForeignKey(s => s.ServiceTypeId);

		}
	}
}