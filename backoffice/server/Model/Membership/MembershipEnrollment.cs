using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class MembershipEnrollment
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public Guid ParentAssociationId { get; set; }
        public Guid MemberId { get; set; }

        [NotMapped]
        public string EnrollmentDate => DateOfEnrollment.ToString("yyyy-MM-dd");
        public DateTime DateOfEnrollment { get; set; }
        public StatusEnum MembershipStatus { get; set; } = StatusEnum.Active;

        public MembershipWorkflowEnum RenewalStatus { get; set; } = MembershipWorkflowEnum.Accepted;

        public DocumentReviewStatusEnum DocumentStatus { get; set; }
        [NotMapped]
        public string RenewalDate => LastRenewalDate.Year == DateTime.MaxValue.AddYears(-999).Year ? "": LastRenewalDate.ToString("yyyy-MM-dd");
        public DateTime LastRenewalDate { get; set; } = DateTime.Today;
        [NotMapped]
        public string DateOfExpiry => ExpiryDate.Year == DateTime.MaxValue.AddYears(-999).Year ? "" : ExpiryDate.ToString("yyyy-MM-dd");
        public DateTime ExpiryDate { get; set; } = DateTime.Today.AddYears(1);

        public decimal ApplicationFee { get; set; }

        public decimal AnnualPayment { get; set; }

        public string InvoiceNo { get; set; }
        public string BankReceiptNo { get; set; }

        public string Remark { get; set; }
        public virtual ICollection<RenewalHistory> RenewalHistory { get; set; }

        public virtual Stakeholder Stakeholder { get; set; }
        public virtual Association Association { get; set; }

        [NotMapped]
        public Individual Member => Stakeholder == null? null: Stakeholder.Individual;

        [NotMapped]
        public String Name
        {
            get
            {
                if (Member == null) return string.Empty;
                return $"{(string.IsNullOrEmpty(Member.NameEng) ? string.Empty : Member.NameEng)} {(string.IsNullOrEmpty(Member.NameLocal) ? string.Empty : Member.NameLocal)}";
            }
        }

        public Guid LastRenewalId { get; set; }

        public IdCardInfo IdCard { get; set; } = new IdCardInfo();
    }
    public class IdCardInfo
    {
        [NotMapped]
        public Guid IndividualId { get; set; } = Guid.Empty;
        [NotMapped]
        public Guid AssociationId { get; set; } = Guid.Empty;
        public String Code { get; set; } = String.Empty;
        public String MembershipNo { get; set; } = String.Empty;
        public String FullName { get; set; } = String.Empty;
        public String CompanyName { get; set; } = String.Empty;
        public String TitleName { get; set; } = String.Empty;
        public String Zone { get; set; } = String.Empty;
        public String Woreda { get; set; } = String.Empty;
        public String HouseNo { get; set; } = String.Empty;
        public String Email { get; set; } = String.Empty;
        public String LicenseNo { get; set; } = String.Empty;
        public String PhoneNumber { get; set; } = String.Empty;
        public String Sector { get; set; } = String.Empty;
        public String DateIssued { get; set; } = String.Empty;
        public String ExpiryDate { get; set; } = String.Empty;
        public bool ReadyForPrint { get; set; } = false;
    }

    public class MembershipEnrollmentPublicModel
    {
        public Guid AssociationId { get; set; }
        public string Code { get; set; }
        public string MemberSince { get; set; }
        public string RenewedOn { get; set; }
        public string ExpiresOn { get; set; }
        public string InvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        public string Fee { get; set; }
        public string Status { get; set; }
        public string Association { get; set; }
        public string AssociationLocal { get; set; }
        public int AssociationType { get; set; }
        public WorkflowStateEnum? RenewalStatus { get; set; }
        public int? RenewalStatusValue => RenewalStatus != null ? (int)RenewalStatus : null;
    }
}