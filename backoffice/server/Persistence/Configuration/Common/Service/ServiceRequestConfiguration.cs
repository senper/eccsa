using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{

    public class ServiceRequestEntityTypeConfiguration :
        IEntityTypeConfiguration<ServiceRequest>
    {
        public void Configure(EntityTypeBuilder<ServiceRequest> config)
        {
            config.ToTable("service_requests", "services");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

            config.Property(o => o.TIN)
                    .HasColumnName("tin")
                    .IsRequired()
                    .HasMaxLength(20);


            config.Property(o => o.FullName)
                    .HasColumnName("full_name")
                    .HasMaxLength(200)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.EmailAddress)
                    .HasColumnName("email")
                    .IsRequired()
                    .HasMaxLength(200);

            config.Property(o => o.Address)
                    .HasColumnName("address")
                    .IsRequired()
                    .HasMaxLength(200);

            config.Property(o => o.MobileNo)
                    .HasColumnName("mobile")
                    .IsRequired()
                    .HasMaxLength(20);

            config.Property(o => o.ServiceTypeId)
                    .HasColumnName("service_type_id")
                    .IsRequired();

            config.Property(o => o.TrackingNo)
                    .HasColumnName("tracing_no")
                    .IsRequired()
                    .HasMaxLength(20);

            config.Property(o => o.RequestingUserId)
                    .HasColumnName("requesting_user_id")
                    .IsRequired();
            //
            config.Property(o => o.OpponentFullName)
                    .HasColumnName("opponent_full_name")
                    .HasMaxLength(200)
                    .IsRequired(false)
                    .IsUnicode();

            config.Property(o => o.OpponentEmail)
                    .HasColumnName("opponent_email")
                    .IsRequired(false)
                    .HasMaxLength(200);

            config.Property(o => o.OpponentMobileNo)
                    .HasColumnName("opponent_mobile")
                    .IsRequired(false)
                    .HasMaxLength(20);

            config.Property(o => o.OpponentUserId)
                    .HasColumnName("opponent_user_id")
                    .IsRequired(false);

            config.Property(o => o.ArbitratorId)
                .HasColumnName("arbitrator_id")
                .IsRequired(false);
            //
            config.Property(o => o.RequestDate)
                    .HasColumnName("request_date")
                    .HasColumnType("date")
                    .IsRequired();

            config.Property(o => o.InvoiceNo)
                    .HasColumnName("invoice_no")
                    .HasDefaultValue(string.Empty)
                    .HasMaxLength(50)
                    .IsRequired();

            config.Property(o => o.Remark)
                    .HasColumnName("remark")
                    .HasDefaultValue(string.Empty)
                    .HasMaxLength(250)
                    .IsUnicode()
                    .IsRequired();

            config.Property(o => o.Amount)
                    .HasColumnName("amount")
                    .HasDefaultValue(0)
                    .IsRequired();

            config.Property(o => o.PrintReceipt)
                   .HasColumnName("print_receipt")
                   .HasDefaultValue(false)
                   .IsRequired();

            config.Property(o => o.AsMember)
                    .HasColumnName("as_member")
                    .HasDefaultValue(YesNoEnum.NotSet)
                    .IsRequired();

            config.Property(o => o.ReceiptNo)
                    .HasColumnName("receipt_no")
                    .HasDefaultValue(string.Empty)
                    .HasMaxLength(50)
                    .IsRequired();

            config.Property(o => o.PaymentStatus)
                    .HasColumnName("receipt_status")
                    .HasDefaultValue(PaymentStatusEnum.NotSet)
                    .IsRequired();

            config.Property(o => o.PaymentMethod)
                    .HasColumnName("payment_method")
                    .HasDefaultValue(PaymentMethodEnum.NotSet)
                    .IsRequired();

            config.Property(o => o.Signature)
                    .HasColumnName("signature")
                    .HasDefaultValue(string.Empty)
                    .HasMaxLength(500)
                    .IsRequired();

            config.Property(o => o.Extension)
                    .HasColumnName("extension")
                    .HasDefaultValue(string.Empty)
                    .HasMaxLength(10)
                    .IsRequired();

            config.Property(o => o.RequestStatus)
                    .HasColumnName("request_status")
                    .HasDefaultValue(WorkflowStateEnum.NotSet)
                    .IsRequired();

            config.Property(o => o.Description)
                    .HasColumnName("description")
                    .IsUnicode();

            config.Property(o => o.PreviousStep)
                    .HasColumnName("previous_step");

            config.Property(o => o.CurrentStep)
                    .HasColumnName("current_step");

            config.Property(o => o.OperatorName)
                    .HasMaxLength(50)
                    .HasDefaultValue(string.Empty)
                    .HasColumnName("operator_name");

            config.Property(o => o.ApproverName)
                    .HasMaxLength(50)
                    .HasDefaultValue(string.Empty)
                    .HasColumnName("approver_name");

            config.Property(o => o.OperatorTitle)
                    .HasMaxLength(50)
                    .HasDefaultValue(string.Empty)
                    .HasColumnName("operator_title");

            config.Property(o => o.ApproverTitle)
                    .HasMaxLength(50)
                    .HasDefaultValue(string.Empty)
                    .HasColumnName("approver_title");

            config.Property(o => o.AuthorizedDate)
                .HasColumnName("authorized_date");

            config.Property(o => o.ApprovedDate)
                .HasColumnName("approved_date");

            config.Property(o => o.OptionalEntries)
                .HasColumnName("optional_entries")
                .HasColumnType("jsonb")
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

            config.HasMany(o => o.Responses)
                 .WithOne(s => s.ServiceRequest)
                 .HasForeignKey(s => s.ServiceRequestId);

            config.HasMany(o => o.Followups)
                  .WithOne(s => s.ServiceRequest)
                 .HasForeignKey(s => s.ServiceId);

            config.HasMany(o => o.OperatorHistory)
                  .WithOne(s => s.ServiceRequest)
                 .HasForeignKey(s => s.ServiceRequestId);

            config.HasMany(o => o.Attachments)
                  .WithOne(s => s.Request)
                 .HasForeignKey(s => s.ServiceRequestId);

        }
    }


    public class ServiceRequestAttachmentEntityTypeConfiguration :
        IEntityTypeConfiguration<RequestAttachment>
    {
        public void Configure(EntityTypeBuilder<RequestAttachment> config)
        {
            config.ToTable("service_request_attachments", "services");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

            config.Property(o => o.Title)
                    .HasColumnName("title")
                    .IsRequired()
                    .HasMaxLength(100);
            config.Property(o => o.Extension)
                    .HasColumnName("extension")
                    .IsRequired()
                    .HasMaxLength(10);

            config.Property(o => o.IsOpponent)
                .HasColumnName("is_opponent")
                .HasDefaultValue(false)
                .IsRequired();

            config.Property(o => o.ServiceRequestId)
                .HasColumnName("service_request_id")
                .IsRequired();
        }
    }
}