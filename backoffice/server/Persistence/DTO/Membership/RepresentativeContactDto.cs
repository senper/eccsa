using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreateRepresentativeContactInput (
		Guid RepresentativeId
	, int ContactType
	, String Address
	, int Priority
);


public record UpdateRepresentativeContactInput (
	Guid Id
	, Guid RepresentativeId
	, int ContactType
	, String Address
	, int Priority
);




    
    
    public record RepresentativeContactPayload(RepresentativeContact PayLoad);
    
}
