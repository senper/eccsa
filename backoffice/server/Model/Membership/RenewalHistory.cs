using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
	public class RenewalHistory
	{
		public Guid Id { get; set; }
		public Guid EnrollmentId { get; set; }

		

		public DateTime ApplicationDate { get; set; }

		public MembershipWorkflowEnum RenewalStatus { get; set; }
		public DateTime RenewalDate { get; set; }

		[NotMapped]
		public string DateRenewed => RenewalDate > DateTime.Today ? "" : RenewalDate.ToString("yyyy-MM-dd");
		public decimal Fee { get; set; }

		public string InvoiceNo { get; set; }

		public DateTime InvoiceDate { get; set; }
		public string ReceiptNo { get; set; }

		public PaymentMethodEnum PaymentMethod { get; set; }

		public DateTime PaymentDate { get; set; }
		public string Remark { get; set; }


		public virtual MembershipEnrollment MembershipEnrollment { get; set; }

	}
}