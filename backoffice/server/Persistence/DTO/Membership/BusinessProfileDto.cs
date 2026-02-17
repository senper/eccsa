using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreateBusinessProfileInput (
		Guid IndividualId
	, int ProfileAttribureId
	, String AttributeValue
);


public record UpdateBusinessProfileInput (
	Guid Id
	, Guid IndividualId
	, int ProfileAttribureId
	, String AttributeValue
);




    
    
    public record BusinessProfilePayload(BusinessProfile PayLoad);
    
}
