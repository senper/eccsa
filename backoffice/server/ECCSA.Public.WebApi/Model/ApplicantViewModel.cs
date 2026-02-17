using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Payment;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ECCSA.Public.WebApi.Model
{
    public class ApplicantViewModel 
    {
        public Guid Id { get; set; }
        public String NameEng { get; set; }
        public String NameLocal { get; set; }

        public String Name
        {
            get
            {
                return $"{(string.IsNullOrEmpty(NameEng) ? string.Empty : NameEng)} {(string.IsNullOrEmpty(NameLocal) ? string.Empty : NameLocal)}";
            }
        }

        public string DateApplied => ApplicationDate.ToString("yyyy-MM-dd");
        public DateTime ApplicationDate { get; set; }
        public String TaxIdNumber { get; set; }

        public String TradeRegistrationNumber { get; set; }


        public Guid LocationId { get; set; }

        public Guid AssociationId { get; set; }





        public decimal ApplicationFee { get; set; }

        public decimal AnnualPayment { get; set; }

        public string InvoiceNo { get; set; }

        public string Code { get; set; }

        public bool NoInvoiceNo => string.IsNullOrEmpty(InvoiceNo) || InvoiceNo.Length < 4;

        public bool NoPaymentNo => string.IsNullOrEmpty(ReceiptNo) || ReceiptNo.Length < 4;
        public string ReceiptNo { get; set; }

        public string Remark { get; set; }

        public virtual Location Location { get; set; }
        public MembershipWorkflowEnum ApplicationStatus { get; set; }

        public DocumentReviewStatusEnum DocumentStatus { get; set; }

        public virtual ICollection<ApplicantDocument> LegalDocuments { get; set; }
    }
}
