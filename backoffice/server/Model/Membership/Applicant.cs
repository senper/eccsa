using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class Applicant : Entity, INotificationAddress
    {
        public String NameEng { get; set; } = string.Empty;
        public String NameLocal { get; set; } = string.Empty;

        [NotMapped]
        public String Name
        {
            get
            {
                return $"{(string.IsNullOrEmpty(NameEng) ? string.Empty : NameEng)} {(string.IsNullOrEmpty(NameLocal) ? string.Empty : NameLocal)}";
            }
        }

        [NotMapped]
        public string DateApplied => ApplicationDate.ToString("yyyy-MM-dd");

        [NotMapped]
        public DateTime ExpiryDate { get; set; } = DateTime.Today.AddYears(1);
        public DateTime ApplicationDate { get; set; }
       
        

        public Guid AssociatedUserId { get; set; }


        public Guid AssociationId { get; set; }

        public virtual Association Association { get; set; }

        [NotMapped]
        public Association ServiceType
        {
            get { return Association; }
            set { Association = value; }
        }
        [NotMapped]
        public string AssociationName { get; set; }
       
        public decimal ApplicationFee { get; set; }
        [NotMapped]
        public decimal RemainingAmount { get; set; }

        public string InvoiceNo { get; set; } = string.Empty;

        [NotMapped]
        public string Code { get; set; } = string.Empty;

        [NotMapped]
        public bool NoInvoiceNo => string.IsNullOrEmpty(InvoiceNo) || InvoiceNo.Length < 4;

        [NotMapped]
        public bool NoPaymentNo => string.IsNullOrEmpty(ReceiptNo) || ReceiptNo.Length < 4;
        public string ReceiptNo { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.NotSet;
        [NotMapped]
        public int PaymentStatusInt => (int)PaymentStatus;

        public string Remark { get; set; } = string.Empty;
        public WorkflowStateEnum ApplicationStatus { get; set; }
        [NotMapped]
        public WorkflowStateEnum RequestStatus
        {
            get { return ApplicationStatus; }
            set { ApplicationStatus = value; }
        }

        public int ApplicationStatusValue => (int)ApplicationStatus;


        public virtual ICollection<ApplicantDocument> Responses { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }

        public Guid? IndividualId { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public int ApplicationTypeValue => (int)ApplicationType;
        [NotMapped]
        public string Reason => ApplicationType  == ApplicationTypeEnum.Renewal? "Renewal" : "New";

        public string TrackingNo { get; set; }

        public string OperatorName { get; set; } = string.Empty;

        public string ApproverName { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
        public bool PrintReceipt { get; set; }

        [NotMapped]
        public bool UnderReview
        {
            get
            {
                return ApplicationStatus switch
                {
                    WorkflowStateEnum.NotSet => false,
                    WorkflowStateEnum.Submitted => true,
                    WorkflowStateEnum.UnderReview => true,
                    _ => false,
                };
            }
        }
        [NotMapped]
        public bool ReadyForApproval
        {
            get
            {
                return ApplicationStatus switch
                {
                    WorkflowStateEnum.RecommendRejection => true,
                    WorkflowStateEnum.DraftPrepared => true,
                    _ => false,
                };
            }
        }
        [NotMapped]
        public bool Approved
        {
            get
            {
                return ApplicationStatus switch
                {
                    WorkflowStateEnum.Rejected => true,
                    WorkflowStateEnum.Accepted => true,
                    WorkflowStateEnum.Closed => true,
                    _ => false,
                };
            }
        }

        [NotMapped]
        public bool Rejection
        {
            get
            {
                return ApplicationStatus switch
                {
                    WorkflowStateEnum.Rejected => true,
                    WorkflowStateEnum.RecommendRejection => true,
                    _ => false,
                };
            }
        }

        [NotMapped]
        public int RequestStatusValue => (int)ApplicationStatus;

        [NotMapped]
        public int PendingDocumentsNo
        {
            get
            {
                if (Responses == null || Responses.Count == 0) return -1;
                var pendingResponses = Responses.Where(r => r.ReviewStatus != DocumentReviewStatusEnum.Accepted &&
                                        r.ReviewStatus != DocumentReviewStatusEnum.NotApplicable).ToList();
                return pendingResponses.Count;
            }
        }

        [NotMapped]
        public int RejectedDocumentsNo
        {
            get
            {
                if (Responses == null || Responses.Count == 0) return 0;
                return Responses.Where(r => r.ReviewStatus == DocumentReviewStatusEnum.Rejected).Count();
            }
        }
        [NotMapped]
        public int AcceptedDocumentsNo
        {
            get
            {
                if (Responses == null || Responses.Count == 0) return 0;
                return Responses.Where(r => r.ReviewStatus == DocumentReviewStatusEnum.Accepted).Count();
            }
        }
        [NotMapped]
        public int TotalDocumentsNo
        {
            get
            {
                if (Responses == null || Responses.Count == 0) return 0;
                return Responses.Count;
            }
        }

        public string TIN { get; set; }
        public string EmailAddress { get; set; }
        [NotMapped]
        public string Email { get { return EmailAddress; } set { EmailAddress = value; } }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        
        [NotMapped]
        public String FullName
        {
            get { return NameEng; }
            set { NameEng = value; }
        }

        [NotMapped]
        public Invoice Invoice { get; set; }
    }
}
