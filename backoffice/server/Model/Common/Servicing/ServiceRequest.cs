using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class ServiceRequest : Entity, INotificationAddress
    {
        [NotMapped]
        public long CerOfOriginId { get; set; }
        public string Address { get; set; } = "n/a";
        public string TIN { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        [NotMapped]
        public string Email { get { return EmailAddress; } set { EmailAddress = value; } }
        public string MobileNo { get; set; }

        public string TrackingNo { get; set; }
        public Guid RequestingUserId { get; set; }
        public string? OpponentFullName { get; set; }
        public string? OpponentEmail { get; set; }
        public string? OpponentMobileNo { get; set; }
        public Guid? OpponentUserId { get; set; }

        public Guid? ArbitratorId { get; set; }

        [NotMapped]
        public string ArbitratorName { get; set; }

        [NotMapped]
        public bool IsOpponent { get; set; }
        public Guid ServiceTypeId { get; set; }

        public DateTime RequestDate { get; set; }
        [NotMapped]
        public string RequestDateStr => RequestDate.ToString("yyyy-MM-dd");
        public string InvoiceNo { get; set; }

        [NotMapped]
        public Invoice Invoice { get; set; } = null;

        public decimal Amount { get; set; }
        [NotMapped]
        public decimal RemainingAmount { get; set; }
        public String Remark { get; set; }
        public string ReceiptNo { get; set; }
        [NotMapped]
        public string CertificateUrl { get; set; }

        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.NotSet;
        [NotMapped]
        public int PaymentStatusInt => (int)PaymentStatus;

        [NotMapped]
        public string Receipt
        {
            get
            {
                if (string.IsNullOrEmpty(ReceiptNo)) return string.Empty;
                return $"{ReceiptNo}  {ReceiptAmount}";
            }
        }

        [NotMapped]
        public string ReceiptAmount { get; set; }
        [NotMapped]
        public string InvoiceAmount { get; set; }

        [NotMapped]
        public DateTime InvoiceDate { get; set; }

        [NotMapped]
        public DateTime Today { get; set; }



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
                return Responses.Where(r => (r.Requirement == null || r.ServiceRequest == null || r.Requirement.IsForOpponent == r.ServiceRequest.IsOpponent) 
                && r.ReviewStatus == DocumentReviewStatusEnum.Rejected).Count();
            }
        }
        [NotMapped]
        public int AcceptedDocumentsNo
        {
            get
            {
                if (Responses == null || Responses.Count == 0) return 0;
                return Responses.Where(r => (r.Requirement == null || r.ServiceRequest == null || r.Requirement.IsForOpponent == r.ServiceRequest.IsOpponent) && r.ReviewStatus == DocumentReviewStatusEnum.Accepted).Count();
            }
        }
        [NotMapped]
        public int TotalDocumentsNo
        {
            get
            {
                if (Responses == null || Responses.Count == 0) return 0;
                return Responses.Where(r => (r.Requirement == null || r.ServiceRequest == null || r.Requirement.IsForOpponent == r.ServiceRequest.IsOpponent)).Count();
            }
        }
        public string Extension { get; set; }
        public string Signature { get; set; }
        public bool PrintReceipt { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }

        public WorkflowStateEnum RequestStatus { get; set; }

        [NotMapped]
        public bool UnderReview
        {
            get
            {
                return RequestStatus switch
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
                return RequestStatus switch
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
                return RequestStatus switch
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
                return RequestStatus switch
                {
                    WorkflowStateEnum.Rejected => true,
                    WorkflowStateEnum.RecommendRejection => true,
                    _ => false,
                };
            }
        }

        [NotMapped]
        public int RequestStatusValue => (int)RequestStatus;

        public YesNoEnum AsMember { get; set; }
        public string Description { get; set; }

        public Guid? PreviousStep { get; set; }
        public Guid CurrentStep { get; set; }
        public ServiceType ServiceType { get; set; }

        [NotMapped]
        public string AttachmentRequired
        {
            get
            {
                if (ServiceType == null) return " required";
                return ServiceType.ApprovalAttachmentRequired ? " required" : "";
            }
        }

        public string OperatorName { get; set; }
        public string ApproverName { get; set; }
        public string OperatorTitle { get; set; }
        public string ApproverTitle { get; set; }
        public DateTime? AuthorizedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public List<OptionalEntry> OptionalEntries { get; set; }
        public virtual ICollection<ServiceRequirementResponse> Responses { get; set; }

        public virtual ICollection<ServiceFollowup> Followups { get; set; }

        public virtual ICollection<OperatorHistory> OperatorHistory { get; set; }

        public virtual ICollection<RequestAttachment> Attachments { get; set; }
        [NotMapped]
        public Resource Resource { get; set; }
    }
    public class OptionalEntry
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }
    public class OperatorHistory
    {
        public Guid Id { get; set; }

        public Guid ServiceRequestId { get; set; }

        public Guid UserId { get; set; }
        public string OperatorName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual ServiceRequest ServiceRequest { get; set; }
    }
    public class RequestAttachment
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Extension { get; set; }
        public Guid ServiceRequestId { get; set; }
        public bool IsOpponent { get; set; }
        public virtual ServiceRequest Request { get; set; }
        [NotMapped]
        public string RelativePath { get; set; }
    }

}