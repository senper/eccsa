using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
	public record CreateRepresentativeInput (
		Guid MemberId
	, String Title
	, String NameEng
	, String NameLocal
	, string OrderNo
	, StatusEnum Status
	, String Position
	, DateTime StartDate
	, DateTime EndDate
	, Boolean IsPrimary
);


public record UpdateRepresentativeInput (
	Guid Id
	, Guid MemberId
	, String Title
	, String NameEng
	, String NameLocal
	, string OrderNo
	, StatusEnum Status
	, String Position
	, DateTime StartDate
	, DateTime EndDate
	, Boolean IsPrimary
);




	
	
	public record RepresentativePayload(Representative PayLoad);
	
}
