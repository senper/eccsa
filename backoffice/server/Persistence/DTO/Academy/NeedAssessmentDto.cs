using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateNeedAssessmentInput (
		DateTime StartDate
	, DateTime EndDate
	, String Remark
	, int Status
);


public record UpdateNeedAssessmentInput (
	Guid Id
	, DateTime StartDate
	, DateTime EndDate
	, String Remark
	, int Status
);




    
    
    public record NeedAssessmentPayload(NeedAssessment PayLoad);
    
}
