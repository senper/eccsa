using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
	public record CreateLegalDocumentInput (
		Guid IndividualId
	, int LegalDocumentTypeId
	, String Document
	, string Extension
);


public record UpdateLegalDocumentInput (
	Guid Id
	, Guid IndividualId
	, int LegalDocumentTypeId
	, String Document
	, string Extension
	, DocumentReviewStatusEnum Status
	, String Remark
);

	public record UpdateLegalDocumentStatusInput(
	Guid Id
	, Guid EnrollmentId
	, Guid IndividualId
	, DocumentReviewStatusEnum Status
	, String Remark
);




	public record LegalDocumentPayload(LegalDocument PayLoad);
	
}
