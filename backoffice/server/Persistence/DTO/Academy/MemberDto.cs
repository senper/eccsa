using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateMemberInput (
		String NameEng
	, String NameLocal
	, int MemberType
	, Guid MembershipId
	, int AddressType
	, Guid LocationId
	, String AddressLine
	, int ContactType
	, String Contact
);


public record UpdateMemberInput (
	Guid Id
	, String NameEng
	, String NameLocal
	, int MemberType
	, Guid MembershipId
	, int AddressType
	, Guid LocationId
	, String AddressLine
	, int ContactType
	, String Contact
);




    
    
    public record MemberPayload(Member PayLoad);
    
}
