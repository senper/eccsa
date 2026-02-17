using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateYesNoInput (
		String Name
	, String NameLocal
);


public record UpdateYesNoInput (
	int Id
	, String Name
	, String NameLocal
);




    
    
    public record YesNoPayload(YesNo payLoad);
    
}
