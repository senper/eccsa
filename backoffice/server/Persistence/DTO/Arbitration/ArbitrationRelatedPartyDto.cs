using ECCSA.BackOffice.Server.Model.Arbitration;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Arbitration
{
    public record CreateArbitrationRelatedPartyInput (
		Guid ServiceId
	, String OrderNo
	, String Name
	, String TINumber
	, String Address
);


public record UpdateArbitrationRelatedPartyInput (
	Guid Id
	, Guid ServiceId
	, String OrderNo
	, String Name
	, String TINumber
	, String Address
);




    
    
    public record ArbitrationRelatedPartyPayload(ArbitrationRelatedParty PayLoad);
    
}
