using ECCSA.BackOffice.Server.Model.Arbitration;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Arbitration
{
    public record CreateArbitrationTaskTypeInput (
		String NameEng
	, String NameLocal
	, String DescriptionEng
	, String DescriptionLocal
	, Decimal ServiceCharge
);


public record UpdateArbitrationTaskTypeInput (
	Guid Id
	, String NameEng
	, String NameLocal
	, String DescriptionEng
	, String DescriptionLocal
	, Decimal ServiceCharge
);




    
    
    public record ArbitrationTaskTypePayload(ArbitrationTaskType PayLoad);
    
}
