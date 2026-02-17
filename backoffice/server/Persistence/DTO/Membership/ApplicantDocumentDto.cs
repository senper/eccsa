using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{

	public record UpdateApplicantDocumentStatusInput(
		Guid Id
		, Guid ApplicantId
		, DocumentReviewStatusEnum Status
		, String Remark
	);
   

	public record CreateApplicantDocumentInput (
		Guid ApplicantId
	, int LegalDocumentTypeId
);


public record UpdateApplicantDocumentInput (
	Guid Id
	, Guid ApplicantId
	, int LegalDocumentTypeId
	, String Document
	, string Extension
	
);



	public record ApplicationInvoiceInput(
	   Guid Id
	   , Decimal Fee
	   , String InvoiceNo
   );

	public record ApplicationPaymentInput(
	Guid Id
	, String ReceiptNo
	, PaymentMethodEnum PaymentMethod
	

);

	public record ApproveRejectApplicationInput(
 Guid Id
 , MembershipWorkflowEnum RenewalStatus
 , String Remark
 , String Code
);
	public record ApplicantDocumentPayload(ApplicantDocument PayLoad);
	
}
