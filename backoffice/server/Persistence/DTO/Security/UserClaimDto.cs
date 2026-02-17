using ECCSA.BackOffice.Server.Model.Security;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Security
{
    public record CreateUserClaimInput (
		Guid UserId
	, String ClaimType
	, String ClaimValue
);


public record UpdateUserClaimInput (
	Guid Id
	, Guid UserId
	, String ClaimType
	, String ClaimValue
);




    
    
    public record UserClaimPayload(UserClaim payLoad);
    
}
