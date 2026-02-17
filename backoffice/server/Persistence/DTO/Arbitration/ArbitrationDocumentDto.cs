using ECCSA.BackOffice.Server.Model.Arbitration;
using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Arbitration
{
    public record CreateArbitrationDocumentInput (
		Guid ServiceId
	, String OrderNo
	, Guid DocumentTypeId
	, String Name
	, String Content
	, DateTime? DateSubmitted
	//, Guid? ReviewedBy
	//, DateTime? DateReviewed
	//, String Remark
	//, WorkflowStateEnum ReviewStatus
);


public record UpdateArbitrationDocumentInput (
	Guid Id
	, Guid ServiceId
	, String OrderNo
	, Guid DocumentTypeId
	, String Name
	, String Content
	, DateTime? DateSubmitted
	//, Guid? ReviewedBy
	//, DateTime? DateReviewed
	//, String Remark
	//, WorkflowStateEnum ReviewStatus
);

	public record UpdateArbitrationDocumentReviewStatusInput(
	Guid Id
	, Guid ServiceId
//, Guid? ReviewedBy
, DateTime? DateReviewed
, String Remark
, WorkflowStateEnum ReviewStatus
);

	public record ArbitrationDocumentPayload(ArbitrationDocument PayLoad);
    
}
