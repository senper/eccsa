using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
	public record CreateContactInput (
		Guid MemberId
	, ContactTypeEnum ContactType
	, String Address
	, int Priority
);


public record UpdateContactInput (
	Guid Id
	, Guid MemberId
	, ContactTypeEnum ContactType
	, String Address
	, int Priority
);




	
	
	public record ContactPayload(Contact PayLoad);
	
}
