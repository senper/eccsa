using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class ServiceRequirementResponse
    {
        public Guid Id { get; set; }

        public Guid ServiceRequestId { get; set; }

        public Guid RequirementId { get; set; }

        public string TextResponse { get; set; }
        public DateTime? DateResponse { get; set; }
        public decimal? DecimalResponse { get; set; }
        public List<TabularResponse> TabularResponse { get; set; }
        public long? IntResponse { get; set; }

        public DateTime? DateSubmitted { get; set; }

        public bool PrintReceipt { get; set; }

        [NotMapped]
        public String RelativePath
        {
            get; set;
        }

        [NotMapped]
        public string Response
        {
            get
            {
                if(Requirement == null)
                {
                    return "?";
                }
                return Requirement.InputMethod switch
                {
                    ServiceInputTypeEnum.Upload => TextResponse,
                    ServiceInputTypeEnum.Integer => IntResponse?.ToString() ?? "",
                    ServiceInputTypeEnum.Decimal => DecimalResponse?.ToString() ?? "",
                    ServiceInputTypeEnum.Date => DateResponse?.ToString("yyyy-MM-dd") ?? "",
                    ServiceInputTypeEnum.Payment => ReceiptNo ?? "",
                    _ => TextResponse,
                };
            }
        }

        public Guid? LookupValueId { get; set; }
        public string InvoiceNo { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        [NotMapped]
        public Invoice Invoice { get; set; } = null;
        public decimal? InvoiceAmount { get; set; }
        [NotMapped]
        public decimal RemainingAmount { get; set; }
        public DateTime? InvoiceDate { get; set; }
        [NotMapped]
        public string InvoiceDateStr => InvoiceDate?.ToString("yyyy-MM-dd") ?? "";
        public string ReceiptNo { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.NotSet;
        [NotMapped]
        public int PaymentStatusInt => (int)PaymentStatus;
        public decimal? AmountPaid { get; set; }
        public DateTime? DatePaid { get; set; }

        public Guid? ReviewedBy { get; set; }

        public DateTime? DateReviewed { get; set; }
        public string Remark { get; set; }
        public DocumentReviewStatusEnum ReviewStatus { get; set; }
        [NotMapped]
        public int ReviewStatusValue => (int)ReviewStatus;

        public virtual ServiceRequest ServiceRequest { get; set; }

        public virtual ServiceRequirement Requirement { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                if (Requirement == null || Requirement?.InputMethod != ServiceInputTypeEnum.Payment)
                    return Requirement?.Name;
                if (string.IsNullOrEmpty(InvoiceNo))
                    return Requirement?.Name;
                return $"{Requirement?.Name} InvoiceNo: {InvoiceNo} ({InvoiceAmount?.ToString("#,##0.00")}Birr)";
            }
        }
    }

}