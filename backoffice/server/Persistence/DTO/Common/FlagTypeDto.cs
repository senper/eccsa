using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateFlagTypeInput (
		String Name
	, String NameLocal
);


public record UpdateFlagTypeInput (
	int Id
	, String Name
	, String NameLocal
);




    
    
    public record FlagTypePayload(FlagType payLoad);
    
}
