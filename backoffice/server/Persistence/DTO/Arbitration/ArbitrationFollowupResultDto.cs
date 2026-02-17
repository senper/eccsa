using ECCSA.BackOffice.Server.Model.Arbitration;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Arbitration
{
    public record CreateArbitrationFollowupResultInput (
		String NameEng
	, String NameLocal
	, String DescriptionEng
	, String DescriptionLocal
);


public record UpdateArbitrationFollowupResultInput (
	Guid Id
	, String NameEng
	, String NameLocal
	, String DescriptionEng
	, String DescriptionLocal
);




    
    
    public record ArbitrationFollowupResultPayload(ArbitrationFollowupResult PayLoad);
    
}
