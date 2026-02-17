using ECCSA.BackOffice.Server.Model.Arbitration;
using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Arbitration
{
    public record CreateArbitrationServiceTypeInput (
		String NameEng
	, String NameLocal
	, YesNoEnum RequiresThirdParty
	, String DescriptionEng
	, String DescriptionLocal
	, Decimal ServiceCharge
				);


public record UpdateArbitrationServiceTypeInput (
	Guid Id
	, String NameEng
	, String NameLocal
	, YesNoEnum RequiresThirdParty
	, String DescriptionEng
	, String DescriptionLocal
	, Decimal ServiceCharge
				);




    
    
    public record ArbitrationServiceTypePayload(ArbitrationServiceType PayLoad);
    
}
