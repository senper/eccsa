using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
	public record CreateApplicantInput (
		String NameEng
	, String NameLocal
	, DateTime ApplicationDate
	, Guid AssociatedUserId
	, Guid AssociationId
	, Decimal ApplicationFee
	, String InvoiceNo
	, String ReceiptNo
	, String Remark
	, WorkflowStateEnum ApplicationStatus
);


public record UpdateApplicantInput (
	Guid Id
	, String NameEng
	, String NameLocal
	, DateTime ApplicationDate
	, Guid AssociatedUserId
	, Guid AssociationId
	, Decimal ApplicationFee
	, String InvoiceNo
	, String ReceiptNo
	, String Remark
	, WorkflowStateEnum ApplicationStatus
);




	
	
	public record ApplicantPayload(Applicant PayLoad);
	
}
