using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreateMembershipEnrollmentInput (
		String Code
	, Guid ParentAssociationId
	, Guid MemberId
	, DateTime DateOfEnrollment
	, StatusEnum MembershipStatus
	, DateTime LastRenewalDate
	, DateTime ExpiryDate
	, Decimal ApplicationFee
	, Decimal AnnualPayment
	, String InvoiceNo
	, String BankReceiptNo
	, String Remark
);


public record UpdateMembershipEnrollmentInput (
	Guid Id
	, String Code
	, Guid ParentAssociationId
	, Guid MemberId
	, DateTime DateOfEnrollment
	, StatusEnum MembershipStatus
	, DateTime LastRenewalDate
	, DateTime ExpiryDate
	, Decimal ApplicationFee
	, Decimal AnnualPayment
	, String InvoiceNo
	, String BankReceiptNo
	, String Remark
);




    
    
    public record MembershipEnrollmentPayload(MembershipEnrollment PayLoad);
    
}
