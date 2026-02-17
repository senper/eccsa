using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreateAddressInput (
		Guid MemberId
	, int AddressType
	, Guid LocationId
	, String AddressLine
);


public record UpdateAddressInput (
	Guid Id
	, Guid MemberId
	, int AddressType
	, Guid LocationId
	, String AddressLine
);




    
    
    public record AddressPayload(Address PayLoad);
    
}
