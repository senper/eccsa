using ECCSA.BackOffice.Server.Model.Communication;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Communication
{
    public record CreateResourceInput (
		String Title
	, String TitleLocal
	, String Description
	, String DescriptionLocal
);


public record UpdateResourceInput (
	Guid Id
	, String Title
	, String TitleLocal
	, String Description
	, String DescriptionLocal
);




    
    
    public record ResourcePayload(Resource PayLoad);
    
}
