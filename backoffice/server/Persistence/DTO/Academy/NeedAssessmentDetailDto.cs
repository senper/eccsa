using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateNeedAssessmentDetailInput (
		int CategoryId
	, Guid MemberId
	, Guid AssessmentId
	, int NeedRating
	, DateTime BestBefore
	, String Remark
	, int Status
);


public record UpdateNeedAssessmentDetailInput (
	Guid Id
	, int CategoryId
	, Guid MemberId
	, Guid AssessmentId
	, int NeedRating
	, DateTime BestBefore
	, String Remark
	, int Status
);




    
    
    public record NeedAssessmentDetailPayload(NeedAssessmentDetail PayLoad);
    
}
