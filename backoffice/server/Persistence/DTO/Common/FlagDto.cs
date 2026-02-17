using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateFlagInput (
		int FlagTypeId
	, String Name
	, String NameLocal
);


public record UpdateFlagInput (
	int Id
	, int FlagTypeId
	, String Name
	, String NameLocal
);




    
    
    public record FlagPayload(Flag payLoad);
    
}
