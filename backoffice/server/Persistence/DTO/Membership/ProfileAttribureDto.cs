using ECCSA.BackOffice.Server.Model.Membership;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
	public record CreateProfileAttribureInput (
	 String NameEng
	, String NameLocal
	, ProfileAttributeTypeEnum Type
	, int Max
	, int Min
	, List<SimpleLookup> Source
);


public record UpdateProfileAttribureInput (
	int Id
	, String NameEng
	, String NameLocal
	, ProfileAttributeTypeEnum Type
	, int Max
	, int Min
	, List<SimpleLookup> Source
);




	
	
	public record ProfileAttriburePayload(ProfileAttribure PayLoad);
	
}
