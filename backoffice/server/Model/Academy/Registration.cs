using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Academy
{
	public class Registration: Entity, INotificationAddress
	{
		public Guid TrainingId { get; set; }
		public Guid LoginId { get; set; }
		public Guid? RequestId { get; set; }
		public Guid? TrainingSponsorId { get; set; }
		public String FullName { get; set; }
		public String EmailAddress { get; set; }
		public String MobileNo { get; set; }
		public DateTime RegistrationDate { get; set; }
		public RegistrationStatus RegistrationStatus { get; set; }
		public int DiscountCategory { get; set; }
		public int TrainingRating { get; set; }
		public String TrainingFeedback { get; set; }
		public String CertificateAttachment { get; set; }
		public String InvoiceNumber { get; set; }
		public decimal? InvoiceRate { get; set; }
		public YesNoEnum AsMember { get; set; }
		public String Remark { get; set; }
		public string ReceiptNo { get; set; }
		public PaymentMethodEnum PaymentMethod { get; set; }

		[NotMapped]
		public DateTime InvoiceDate { get; set; }

		[NotMapped]
		public DateTime Today { get; set; }

		public virtual ICollection<Attendance> Attendances { get; set; }
public virtual ICollection<ResourceDelivery> ResourceDeliveries { get; set; }
public virtual ICollection<Assessment> Assessments { get; set; }


		public virtual Training Training { get; set; }
public virtual TrainingSponsor TrainingSponsor { get; set; }
		[NotMapped]
        public string Email { get { return EmailAddress; } set { EmailAddress = value; } }
    }
}