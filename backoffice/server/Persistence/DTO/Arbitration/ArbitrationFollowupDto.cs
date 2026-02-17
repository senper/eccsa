using ECCSA.BackOffice.Server.Model.Arbitration;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Arbitration
{
    public record CreateArbitrationFollowupInput (
		Guid ServiceId
	, Guid TaskTypeId
	, DateTime PlannedDate
	, Guid FollowupResultId
	, String Description
	, String Remark
				);


public record UpdateArbitrationFollowupInput (
	Guid Id
	, Guid ServiceId
	, Guid TaskTypeId
	, DateTime PlannedDate
	, Guid FollowupResultId
	, String Description
	, String Remark
				);




    
    
    public record ArbitrationFollowupPayload(ArbitrationFollowup PayLoad);
    
}
