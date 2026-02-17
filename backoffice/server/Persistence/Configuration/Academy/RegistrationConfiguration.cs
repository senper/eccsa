using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class RegistrationEntityTypeConfiguration:
		IEntityTypeConfiguration<Registration>
	{
	  public void Configure(EntityTypeBuilder<Registration> config)
	  {
			config.ToTable("registrations", "training") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.TrainingId)
			.HasColumnName("training_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.LoginId)
			.HasColumnName("login_id")
			.HasColumnType("uuid");

            config.Property(o => o.RequestId)
            .HasColumnName("request_id")
            .HasColumnType("uuid")
			.IsRequired(false);

            config.Property(o => o.TrainingSponsorId)
			.HasColumnName("training_sponsor_id")
			.HasColumnType("uuid");
			

			config.Property(o => o.FullName)
			.HasColumnName("full_name")
			
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();


			config.Property(o => o.EmailAddress)
			.HasColumnName("email_address")
			
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();


			config.Property(o => o.MobileNo)
			.HasColumnName("mobile_no")
			.IsRequired()
			.HasMaxLength(50);


			config.Property(o => o.RegistrationDate)
			.HasColumnName("registration_date")
			.HasColumnType("date")
			.IsRequired();


			config.Property(o => o.RegistrationStatus)
			.HasColumnName("registration_status")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.TrainingRating)
			.HasColumnName("training_rating")
			.HasColumnType("int");
			

			config.Property(o => o.TrainingFeedback)
			.HasColumnName("training_feedback")
			.IsUnicode();


			config.Property(o => o.CertificateAttachment)
			.HasColumnName("certificate_attachment")
			.HasMaxLength(200)
			.IsUnicode();


			config.Property(o => o.InvoiceNumber)
			.HasColumnName("invoice_number")
			.HasMaxLength(50)
			.IsUnicode();

			config.Property(o => o.InvoiceRate)
			.HasColumnName("invoice_rate");

			config.Property(o => o.AsMember)
					.HasColumnName("as_member")
					.HasDefaultValue(YesNoEnum.NotSet)
					.IsRequired();

			config.Property(o => o.ReceiptNo)
					.HasColumnName("receipt_no")
					.HasDefaultValue(string.Empty)
					.HasMaxLength(50)
					.IsRequired();

			config.Property(o => o.PaymentMethod)
					.HasColumnName("payment_method")
					.HasDefaultValue(PaymentMethodEnum.NotSet)
					.IsRequired();

			config.Property(o => o.Remark)
					.HasColumnName("remark")
					.HasDefaultValue(string.Empty)
					.HasMaxLength(250)
					.IsUnicode()
					.IsRequired();

			config.Property(o => o.InvoiceRate)
			.HasColumnName("invoice_rate");

			config.Property(o => o.DiscountCategory)
			.HasColumnName("discount_category")
			.IsRequired();


			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o=> o.CreateStamp)
				.HasColumnName("create_stamp")
				.HasDefaultValue(DateTime.UtcNow)
				.IsRequired();

			config.Property(o => o.UpdatedBy)
				.HasColumnName("updated_by")
				.IsRequired();

			config.Property(o => o.UpdateStamp)
				.HasColumnName("update_stamp")
				.HasDefaultValue(DateTime.UtcNow)
				.IsRequired();

			config.HasMany<Assessment>(o => o.Assessments)
				.WithOne(s => s.Registration)
				.HasForeignKey(s => s.RegistrationId);


			config.HasMany<Attendance>(o => o.Attendances)
				.WithOne(s => s.Registration)
				.HasForeignKey(s => s.RegistrationId);


			config.HasMany<ResourceDelivery>(o => o.ResourceDeliveries)
				.WithOne(s => s.Registration)
				.HasForeignKey(s => s.RegistrationId);
	  }
	}
}